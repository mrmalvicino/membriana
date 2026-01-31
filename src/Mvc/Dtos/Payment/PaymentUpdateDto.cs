namespace Mvc.Dtos.Payment
{
    /// <summary>
    /// Request DTO para la modificación de un pago.
    /// </summary>
    public class PaymentUpdateDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }
}
