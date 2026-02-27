using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
    {

    }

    protected override IQueryable<Employee> IncludeRelations(IQueryable<Employee> query)
    {
        return query
            .Include(m => m.ProfileImage);
    }
}
