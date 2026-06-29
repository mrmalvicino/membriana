using System.ComponentModel.DataAnnotations;

namespace Mvc.Areas.Admin.ViewModels;

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Código")]
    public string ReferenceCode { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Confirmado")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Rol")]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    public string? LinkedPersonName { get; set; }

    [Display(Name = "Tipo")]
    public string? LinkedPersonType { get; set; }
}
