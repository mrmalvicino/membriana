using Domain.Interfaces;

namespace Application.Dtos.Payment
{
    public class PaymentCreateDto : ITenantable
    {
        public decimal Amount { get; set; }
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }
}
