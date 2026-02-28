using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email del administrador")]
    public string UserEmail { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmPassword { get; set; } = null!;

    public string OrganizationName { get; set; } = null!;
    public string OrganizationEmail { get; set; } = null!;
}
