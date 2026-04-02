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

    public async Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month)
    {
        return await _dbSet
            .Where(payment =>
                payment.OrganizationId == organizationId &&
                payment.DateTime.Year == year &&
                payment.DateTime.Month == month &&
                payment.Active
            )
            .SumAsync(payment => (decimal?)payment.Amount) ?? 0m;
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
