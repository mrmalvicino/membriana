using Application.Dtos.Person;

namespace Application.Dtos.Member
{
    public class MemberCreateDto : PersonCreateDto
    {
        public int MembershipPlanId { get; set; }
        public int OrganizationId { get; set; }
    }
}
