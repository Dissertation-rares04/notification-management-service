using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationManagementService.Business.Interface;
using NotificationManagementService.Core.AppSettings;
using System.Text;

namespace NotificationManagementService.Business.Implementation
{
    public class Auth0HttpClient : IAuth0HttpClient
    {
        private readonly IOptions<Auth0Settings> _config;
        private readonly HttpClient _httpClient;
        private static string? _accessToken = null;
        private static DateTime? _accessTokenExpiration = null;

        public Auth0HttpClient(IOptions<Auth0Settings> config)
        {
            _config = config;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{config.Value.Domain}/")
            };
        }

        public async Task<string> GetAccessToken()
        {
            if (_accessToken == null || DateTime.UtcNow >= _accessTokenExpiration)
            {
                var tokenRequest = new
                {
                    client_id = _config.Value.ClientId,
                    client_secret = _config.Value.ClientSecret,
                    audience = _config.Value.Audience,
                    grant_type = "client_credentials"
                };

                var tokenRequestJson = JsonConvert.SerializeObject(tokenRequest);
                var content = new StringContent(tokenRequestJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"https://{_config.Value.Domain}/oauth/token", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to obtain access token. Status code: {response.StatusCode}");
                }

                var tokenResponseJson = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeAnonymousType(tokenResponseJson, new { access_token = string.Empty, expires_in = default(double) });

                _accessToken = tokenResponse.access_token;
                _accessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in);
            }

            return _accessToken;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var accessToken = await GetAccessToken();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"{_config.Value.Audience}{endpoint}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }

        public async Task<T> PostAsync<T>(string endpoint, object content)
        {
            var accessToken = await GetAccessToken();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var contentJson = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_config.Value.Audience}{endpoint}", stringContent);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}
