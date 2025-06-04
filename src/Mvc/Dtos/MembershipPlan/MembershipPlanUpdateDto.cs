namespace Mvc.Dtos.MembershipPlan
{
    public class MembershipPlanUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public int OrganizationId { get; set; }
    }
}
