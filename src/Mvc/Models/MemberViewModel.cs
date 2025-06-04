using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class MemberViewModel : PersonViewModel
    {
        [Display(Name = "Fecha de admisión")]
        [Required(ErrorMessage = "La fecha de admisión es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar un plan.")]
        [Display(Name = "Membresía")]
        public int MembershipPlanId { get; set; }

        public virtual MembershipPlanViewModel? MembershipPlan { get; set; }

        [Required]
        public int OrganizationId { get; set; }
    }
}
