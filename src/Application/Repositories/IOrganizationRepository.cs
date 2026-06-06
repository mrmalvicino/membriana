using Domain.Entities;

namespace Application.Repositories;

public interface IOrganizationRepository : IBaseRepository<Organization>
{
    Task<string?> GetReferenceCodeByIdAsync(int id);
}
