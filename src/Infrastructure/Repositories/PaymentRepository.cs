using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        /// <summary>
        /// Constructor principal.
        /// </summary>
        public PaymentRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
