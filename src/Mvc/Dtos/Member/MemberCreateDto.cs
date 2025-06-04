using Mvc.Dtos.Person;

namespace Mvc.Dtos.Member
{
    public class MemberCreateDto : PersonCreateDto
    {
        public int MembershipPlanId { get; set; }
        public int OrganizationId { get; set; }
    }
}
