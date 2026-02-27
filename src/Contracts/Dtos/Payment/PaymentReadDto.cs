using Contracts.Dtos.Member;
using Contracts.Interfaces;

namespace Contracts.Dtos.Payment;

/// <summary>
/// Response DTO para la lectura de un pago.
/// </summary>
public class PaymentReadDto : IIdentifiable
{
    public int Id { get; set; }
    public bool Active { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public virtual MemberReadDto Member { get; set; } = null!;
    public int OrganizationId { get; set; }
}
