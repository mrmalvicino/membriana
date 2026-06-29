using Domain.Entities;

namespace Application.Repositories;

public interface IAppUserRepository
{
    Task<List<AppUser>> GetAllByOrganizationAsync(int organizationId);
    Task<AppUser?> GetByIdAsync(string id, int organizationId);
}
