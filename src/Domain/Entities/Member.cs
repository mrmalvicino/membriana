using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Member : Person
    {
        [Display(Name = "Fecha de admisión")]
        public DateTime AdmissionDate { get; set; }

        public int MembershipPlanId { get; set; }
        [Required]
        [Display(Name = "Tipo de membresía")]
        public virtual MembershipPlan MembershipPlan { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;
    }
}
