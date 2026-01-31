using Domain.Interfaces;

namespace Application.Dtos.Payment
{
    /// <summary>
    /// Request DTO para la modificación de un pago.
    /// </summary>
    public class PaymentUpdateDto : IIdentifiable
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int OrganizationId { get; set; }
    }
}
