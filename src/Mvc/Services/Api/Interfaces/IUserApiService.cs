using Contracts.Dtos.Authentication;

namespace Mvc.Services.Api.Interfaces;

public interface IUserApiService
{
    Task<LoggedUserContextDto> GetLoggedUserContextAsync();
    Task<int> GetOrganizationIdAsync();
}
