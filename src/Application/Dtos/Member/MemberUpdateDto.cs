using Application.Dtos.Person;

namespace Application.Dtos.Member
{
    public class MemberUpdateDto : PersonUpdateDto
    {
        public DateTime AdmissionDate { get; set; }
        public int MembershipPlanId { get; set; }
        public int OrganizationId { get; set; }
    }
}
