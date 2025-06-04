namespace Mvc.Dtos.MembershipPlan
{
    public class MembershipPlanCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public int OrganizationId { get; set; }
    }
}
