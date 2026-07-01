using System.ComponentModel.DataAnnotations;

namespace Mvc.Areas.Admin.ViewModels;

public class UserCandidateViewModel
{
    public int Id { get; set; }

    [Display(Name = "Código")]
    public string ReferenceCode { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
