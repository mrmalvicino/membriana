using Contracts.Dtos.Authentication;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;
using Contracts.Dtos.User;

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

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener el usuario autenticado.");

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

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener la organización del usuario.");

        var orgId = await response.Content.ReadFromJsonAsync<int?>();

        if (!orgId.HasValue)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }

        return orgId.Value;
    }

    public async Task<List<UserViewModel>> GetAllAsync()
    {
        var url = $"{_apiBaseUrl}api/users";
        var response = await _httpClient.GetAsync(url);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener la lista de usuarios.");

        var dtos = await response.Content.ReadFromJsonAsync<List<UserReadDto>>() ?? new();

        return dtos.Select(
            dto => new UserViewModel
            {
                Id = dto.Id,
                ReferenceCode = dto.ReferenceCode,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed,
                Role = dto.Role,
                LinkedPersonName = dto.LinkedPersonName,
                LinkedPersonType = dto.LinkedPersonType
            }
        ).ToList();
    }

    public async Task<List<UserCandidateViewModel>> GetEligibleMembersAsync()
    {
        var url = $"{_apiBaseUrl}api/users/eligible-members";
        var response = await _httpClient.GetAsync(url);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener la lista de socios disponibles.");

        var dtos = await response.Content.ReadFromJsonAsync<List<UserCandidateDto>>() ?? new();

        return dtos.Select(MapCandidate).ToList();
    }

    public async Task<List<UserCandidateViewModel>> GetEligibleEmployeesAsync()
    {
        var url = $"{_apiBaseUrl}api/users/eligible-employees";
        var response = await _httpClient.GetAsync(url);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener la lista de empleados disponibles.");

        var dtos = await response.Content.ReadFromJsonAsync<List<UserCandidateDto>>() ?? new();

        return dtos.Select(MapCandidate).ToList();
    }

    public async Task<string> CreateUserForMemberAsync(int memberId)
    {
        var url = $"{_apiBaseUrl}api/users/members/{memberId}";
        var response = await _httpClient.PostAsync(url, null);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo crear el usuario para el socio.");

        var dto = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();

        return dto?.Message ?? "Se creó el usuario para el socio.";
    }

    public async Task<string> CreateUserForEmployeeAsync(int employeeId)
    {
        var url = $"{_apiBaseUrl}api/users/employees/{employeeId}";
        var response = await _httpClient.PostAsync(url, null);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo crear el usuario para el empleado.");

        var dto = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();

        return dto?.Message ?? "Se creó el usuario para el empleado.";
    }

    public async Task<UserViewModel?> GetByIdAsync(string id)
    {
        var url = $"{_apiBaseUrl}api/users/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener el usuario.");

        var dto = await response.Content.ReadFromJsonAsync<UserReadDto>();

        if (dto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al obtener el usuario.");
        }

        return new UserViewModel
        {
            Id = dto.Id,
            ReferenceCode = dto.ReferenceCode,
            Email = dto.Email,
            EmailConfirmed = dto.EmailConfirmed,
            Role = dto.Role,
            LinkedPersonName = dto.LinkedPersonName,
            LinkedPersonType = dto.LinkedPersonType
        };
    }

    public async Task DeleteAsync(string id)
    {
        var url = $"{_apiBaseUrl}api/users/{id}";
        var response = await _httpClient.DeleteAsync(url);
        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo eliminar el usuario.");
    }

    private static UserCandidateViewModel MapCandidate(UserCandidateDto dto)
    {
        return new UserCandidateViewModel
        {
            Id = dto.Id,
            ReferenceCode = dto.ReferenceCode,
            Name = dto.Name,
            Email = dto.Email
        };
    }
}
