using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels;

public class MembershipPlanViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El monto de cuota es obligatorio.")]
    [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "El monto debe ser un número positivo.")]
    [Display(Name = "Monto de cuota")]
    public decimal Amount { get; set; }

    [Required]
    public int OrganizationId { get; set; }
}
