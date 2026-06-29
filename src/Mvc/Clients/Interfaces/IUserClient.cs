using Contracts.Dtos.Authentication;
using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IUserClient
{
    Task<LoggedUserContextDto> GetLoggedUserContextAsync();
    Task<int> GetOrganizationIdAsync();
    Task<List<UserViewModel>> GetAllAsync();
    Task<UserViewModel?> GetByIdAsync(string id);
    Task DeleteAsync(string id);
}
