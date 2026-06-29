using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _dbContext;

    public AppUserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AppUser>> GetAllByOrganizationAsync(int organizationId)
    {
        return await _dbContext.Users
            .Include(user => user.Employee)
            .Include(user => user.Member)
            .Where(user => user.OrganizationId == organizationId)
            .OrderBy(user => user.Email)
            .ToListAsync();
    }

    public async Task<AppUser?> GetByIdAsync(string id, int organizationId)
    {
        return await _dbContext.Users
            .Include(user => user.Employee)
            .Include(user => user.Member)
            .FirstOrDefaultAsync(user => user.OrganizationId == organizationId && user.Id == id);
    }
}
