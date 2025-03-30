using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Member : Person, IIdentifiable
    {
        [Display(Name = "Fecha de admisión")]
        [Required]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Membresía")]
        [Required]
        public int MembershipPlanId { get; set; }
        [Display(Name = "Membresía")]
        [ValidateNever]
        public virtual MembershipPlan MembershipPlan { get; set; } = null!;

        [ValidateNever]
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
    }
}
