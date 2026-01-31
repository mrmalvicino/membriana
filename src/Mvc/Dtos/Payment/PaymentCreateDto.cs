namespace Mvc.Dtos.Payment
{
    /// <summary>
    /// Request DTO para la creación de un pago.
    /// </summary>
    public class PaymentCreateDto
    {
        public decimal Amount { get; set; }
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }
}
