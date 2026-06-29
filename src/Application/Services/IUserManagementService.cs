using Contracts.Dtos.User;

namespace Application.Services;

public interface IUserManagementService
{
    Task<List<UserReadDto>> GetAllAsync(int organizationId);
    Task<UserReadDto?> GetByIdAsync(string id, int organizationId);
    Task DeleteAsync(string id, int organizationId, string loggedUserId);
}
