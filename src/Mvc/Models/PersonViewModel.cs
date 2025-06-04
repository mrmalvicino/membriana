using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [Display(Name = "DNI")]
        public string? Dni { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int? ProfileImageId { get; set; }
        public virtual ImageViewModel? ProfileImage { get; set; }
    }
}
