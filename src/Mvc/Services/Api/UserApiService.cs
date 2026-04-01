using Contracts.Dtos.Authentication;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api;

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

    public async Task<LoggedUserContextDto> GetLoggedUserContextAsync()
    {
        var url = $"{_apiBaseUrl}api/users/me";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<LoggedUserContextDto>();
        
        if (dto is null)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }
        
        return dto;
    }

    public async Task<int> GetOrganizationIdAsync()
    {
        var url = $"{_apiBaseUrl}api/users/me/organization-id";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var orgId = await response.Content.ReadFromJsonAsync<int?>();

        if (!orgId.HasValue)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }

        return orgId.Value;
    }
}
