using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }

        [Display(Name = "Estado")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        public int? LogoImageId { get; set; }
        public virtual Image? LogoImage { get; set; }

        public int PricingPlanId { get; set; }
        [Required]
        public virtual PricingPlan PricingPlan { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public virtual ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
    }
}
