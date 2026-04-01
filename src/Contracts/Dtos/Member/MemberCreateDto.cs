using Contracts.Dtos.Person;
using Contracts.Interfaces;
using MemberStatusEnum = Contracts.Enums.MemberStatus;

namespace Contracts.Dtos.Member;

public class MemberCreateDto : PersonCreateDto, ITenantable
{
    public int MembershipPlanId { get; set; }
    public int OrganizationId { get; set; }
    public MemberStatusEnum MemberStatus { get; set; } = MemberStatusEnum.Active;
}
