using Application.Dtos.Member;
using Domain.Interfaces;

namespace Application.Dtos.Payment
{
    public class PaymentReadDto : IIdentifiable
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public virtual MemberReadDto Member { get; set; } = null!;
    }
}
