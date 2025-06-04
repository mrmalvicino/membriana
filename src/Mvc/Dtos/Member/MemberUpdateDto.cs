using Mvc.Dtos.Person;

namespace Mvc.Dtos.Member
{
    public class MemberUpdateDto : PersonUpdateDto
    {
        public DateTime AdmissionDate { get; set; }
        public int MembershipPlanId { get; set; }
        public int OrganizationId { get; set; }
    }
}
