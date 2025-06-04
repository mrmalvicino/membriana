using Mvc.Services.Interfaces;
using System.Net.Http.Headers;

public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetOrganizationIdAsync()
    {
        var response = await _httpClient.GetAsync("api/users/me/organization-id");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}
