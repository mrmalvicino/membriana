using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PricingPlan
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Cuota")]
        public decimal Fee { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
