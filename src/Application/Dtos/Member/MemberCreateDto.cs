using Application.Dtos.Person;
using Domain.Interfaces;

namespace Application.Dtos.Member
{
    public class MemberCreateDto : PersonCreateDto, ITenantable
    {
        public int MembershipPlanId { get; set; }
        public int OrganizationId { get; set; }
    }
}
