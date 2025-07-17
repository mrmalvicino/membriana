using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Authentication
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [DefaultValue("admin@mail.com")]
        public string Email { get; set; } = "admin@mail.com";

        [Required]
        [DefaultValue("Password123-")]
        public string Password { get; set; } = "Password123-";
    }
}