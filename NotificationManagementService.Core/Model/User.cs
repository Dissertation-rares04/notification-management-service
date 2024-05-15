using Newtonsoft.Json;

namespace NotificationManagementService.Core.Model
{
    public class User
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("identities")]
        public List<Identity> Identities { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("last_ip")]
        public string LastIp { get; set; }

        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        [JsonProperty("logins_count")]
        public int LoginsCount { get; set; }
    }

    public class Identity
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("connection")]
        public string Connection { get; set; }

        [JsonProperty("isSocial")]
        public bool IsSocial { get; set; }
    }
}
