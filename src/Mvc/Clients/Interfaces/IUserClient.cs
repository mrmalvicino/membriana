using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;
using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IUserClient
{
    Task<LoggedUserContextDto> GetLoggedUserContextAsync();
	Task<UserProfileReadDto> GetUserProfileAsync();
	Task<UserProfileReadDto> UpdateUserProfileAsync(UserProfileUpdateDto profileUpdateDto);
    Task<int> GetOrganizationIdAsync();
    Task<List<UserViewModel>> GetAllAsync();
    Task<List<UserCandidateViewModel>> GetEligibleMembersAsync();
    Task<List<UserCandidateViewModel>> GetEligibleEmployeesAsync();
    Task<string> CreateUserForMemberAsync(int memberId);
    Task<string> CreateUserForEmployeeAsync(int employeeId);
    Task<UserViewModel?> GetByIdAsync(string id);
    Task DeleteAsync(string id);
}
