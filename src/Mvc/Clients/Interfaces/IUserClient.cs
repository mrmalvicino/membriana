using Contracts.Dtos.Authentication;

namespace Mvc.Clients.Interfaces;

public interface IUserClient
{
    Task<LoggedUserContextDto> GetLoggedUserContextAsync();
    Task<int> GetOrganizationIdAsync();
}
