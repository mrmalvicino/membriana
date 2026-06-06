using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(AppDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<string?> GetReferenceCodeByIdAsync(int id)
    {
        return await _dbSet
            .Where(o => o.Id == id)
            .Select(o => o.ReferenceCode)
            .FirstOrDefaultAsync();
    }
}
