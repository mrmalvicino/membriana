using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "El email del administrador es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email del administrador no tiene un formato válido.")]
    [Display(Name = "Email del administrador")]
    public string UserEmail { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "La confirmación de contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmPassword { get; set; } = null!;

    public string OrganizationName { get; set; } = null!;
    public string OrganizationEmail { get; set; } = null!;
}
