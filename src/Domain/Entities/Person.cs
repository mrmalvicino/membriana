using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Display(Name = "Estado")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [Display(Name = "DNI")]
        public string? Dni { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDate { get; set; }

        public int? ProfileImageId { get; set; }
        public virtual Image? ProfileImage { get; set; }
    }
}
