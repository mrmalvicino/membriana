using Contracts.Dtos.MembershipPlan;
using Contracts.Dtos.Person;

namespace Contracts.Dtos.Member;

public class MemberReadDto : PersonReadDto
{
    public DateTime AdmissionDate { get; set; } = DateTime.Now;
    public MembershipPlanReadDto MembershipPlan { get; set; } = null!;
    public int OrganizationId { get; set; }
}
