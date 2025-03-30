using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingresar el email.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ingresar la contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;
    }
}
