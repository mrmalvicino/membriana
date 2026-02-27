using Contracts.Interfaces;

namespace Contracts.Dtos.Payment;

/// <summary>
/// Request DTO para la creación de un pago.
/// </summary>
public class PaymentCreateDto : ITenantable
{
    public decimal Amount { get; set; }
    public int MemberId { get; set; }
    public int OrganizationId { get; set; }
}
