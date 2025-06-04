using Mvc.Dtos.MembershipPlan;
using Mvc.Dtos.Person;

namespace Mvc.Dtos.Member
{
    public class MemberReadDto : PersonReadDto
    {
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public MembershipPlanReadDto MembershipPlan { get; set; } = null!;
        public int OrganizationId { get; set; }
    }
}
