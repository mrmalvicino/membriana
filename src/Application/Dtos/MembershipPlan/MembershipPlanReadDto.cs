using Domain.Interfaces;

namespace Application.Dtos.MembershipPlan
{
    public class MembershipPlanReadDto : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public int OrganizationId { get; set; }
    }
}
