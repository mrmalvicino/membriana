using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Organization : IIdentifiable
    {
        public int Id { get; set; }

        [Display(Name = "Estado")]
        [Required]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        public int? LogoImageId { get; set; }
        [ValidateNever]
        public virtual Image? LogoImage { get; set; }

        [Required]
        public int PricingPlanId { get; set; }
        [Required]
        public virtual PricingPlan PricingPlan { get; set; } = null!;

        [ValidateNever]
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        [ValidateNever]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        [ValidateNever]
        public virtual ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
    }
}
