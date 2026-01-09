using Domain.Interfaces;

namespace Application.Dtos.Payment
{
    public class PaymentUpdateDto : IIdentifiable
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }
}
