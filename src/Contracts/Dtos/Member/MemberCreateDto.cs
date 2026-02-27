using Contracts.Dtos.Person;
using Contracts.Interfaces;

namespace Contracts.Dtos.Member;

public class MemberCreateDto : PersonCreateDto, ITenantable
{
    public int MembershipPlanId { get; set; }
    public int OrganizationId { get; set; }
}
