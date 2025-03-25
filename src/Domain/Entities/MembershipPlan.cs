using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MembershipPlan
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Cuota")]
        public decimal Fee { get; set; }

        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;
    }
}
