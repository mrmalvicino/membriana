using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;

namespace Application.Services;

public interface IUserManagementService
{
    Task<List<UserReadDto>> GetAllAsync(int organizationId);
    Task<UserReadDto?> GetByIdAsync(string id, int organizationId);
    Task<List<UserCandidateDto>> GetEligibleMembersAsync(int organizationId);
    Task<List<UserCandidateDto>> GetEligibleEmployeesAsync(int organizationId);
    Task<RegisterResponseDto> CreateUserForMemberAsync(int memberId, int organizationId);
    Task<RegisterResponseDto> CreateUserForEmployeeAsync(int employeeId, int organizationId);
    Task DeleteAsync(string id, int organizationId, string loggedUserId);
}
