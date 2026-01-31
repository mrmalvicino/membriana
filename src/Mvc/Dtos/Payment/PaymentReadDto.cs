using Mvc.Dtos.Member;

namespace Mvc.Dtos.Payment
{
    /// <summary>
    /// Response DTO para la lectura de un pago.
    /// </summary>
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public virtual MemberReadDto Member { get; set; } = null!;
    }
}
