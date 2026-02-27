using Contracts.Interfaces;

namespace Contracts.Dtos.MembershipPlan;

public class MembershipPlanCreateDto : ITenantable
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public int OrganizationId { get; set; }
}
