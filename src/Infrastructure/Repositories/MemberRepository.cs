using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MemberRepository : BaseRepository<Member>, IMemberRepository
{
    public MemberRepository(AppDbContext dbContext) : base(dbContext)
    {

    }

    protected override IQueryable<Member> IncludeRelations(IQueryable<Member> query)
    {
        return query
            .Include(m => m.MembershipPlan)
            .Include(m => m.ProfileImage);
    }
}
