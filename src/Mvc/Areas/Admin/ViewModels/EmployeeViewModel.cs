using System.ComponentModel.DataAnnotations;

namespace Mvc.Areas.Admin.ViewModels;

public class EmployeeViewModel : PersonViewModel
{
    [Display(Name = "Fecha de admisión")]
    [Required(ErrorMessage = "La fecha de admisión es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime AdmissionDate { get; set; } = DateTime.Now;

    [Required]
    public int OrganizationId { get; set; }
}
