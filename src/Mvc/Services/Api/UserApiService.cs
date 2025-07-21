using Mvc.Services.Api.Interfaces;

public class UserApiService : IUserApiService
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;

    public UserApiService(
        IConfiguration configuration,
        HttpClient httpClient
    )
    {
        _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        _httpClient = httpClient;
    }

    public async Task<int> GetOrganizationIdAsync()
    {
        var url = $"{_apiBaseUrl}api/users/me/organization-id";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}
