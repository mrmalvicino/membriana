using Application.Dtos.MembershipPlan;
using Application.Dtos.Person;

namespace Application.Dtos.Member
{
    public class MemberReadDto : PersonReadDto
    {
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public MembershipPlanReadDto MembershipPlan { get; set; } = null!;
        public int OrganizationId { get; set; }
    }
}
