using Mvc.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Mvc.Areas.Admin.ViewModels;

public class EmployeeViewModel : PersonViewModel
{
    [Display(Name = "Fecha de admisión")]
    [Required(ErrorMessage = "La fecha de admisión es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime AdmissionDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La organización es obligatoria.")]
    public int OrganizationId { get; set; }
}
