using Contracts.Dtos.Person;
using MemberStatusEnum = Contracts.Enums.MemberStatus;

namespace Contracts.Dtos.Member;

public class MemberUpdateDto : PersonUpdateDto
{
    public DateTime AdmissionDate { get; set; }
    public int MembershipPlanId { get; set; }
    public int OrganizationId { get; set; }
    public MemberStatusEnum MemberStatus { get; set; }
}
