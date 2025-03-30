using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MembershipPlan : IIdentifiable
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Monto de cuota")]
        public decimal Amount { get; set; }

        [ValidateNever]
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
    }
}
