using Contracts.Dtos.Authentication;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;

namespace Mvc.Clients;

public class UserClient : IUserClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;

    public UserClient(
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

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener el usuario autenticado.");

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

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener la organización del usuario.");

        var orgId = await response.Content.ReadFromJsonAsync<int?>();

        if (!orgId.HasValue)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }

        return orgId.Value;
    }
}
