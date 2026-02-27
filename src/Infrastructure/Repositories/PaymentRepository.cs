using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    /// <summary>
    /// Constructor principal.
    /// </summary>
    public PaymentRepository(AppDbContext dbContext) : base(dbContext)
    {

    }

    /// <inheritdoc />
    protected override IQueryable<Payment> IncludeRelations(IQueryable<Payment> query)
    {
        return query
            .Include(p => p.Member)
                .ThenInclude(m => m.MembershipPlan)
            .Include(p => p.Member)
                .ThenInclude(m => m.ProfileImage);
    }
}
