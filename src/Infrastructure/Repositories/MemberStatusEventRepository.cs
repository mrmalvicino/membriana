using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class MemberStatusEventRepository : BaseRepository<MemberStatusEvent>, IMemberStatusEventRepository
{
    public MemberStatusEventRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}