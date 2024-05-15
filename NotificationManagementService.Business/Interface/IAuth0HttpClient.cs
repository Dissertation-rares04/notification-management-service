namespace NotificationManagementService.Business.Interface
{
    public interface IAuth0HttpClient
    {
        Task<string> GetAccessToken();

        Task<T> GetAsync<T>(string endpoint);

        Task<T> PostAsync<T>(string endpoint, object content);
    }
}
