using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        public string? Email { get; set; }
    }
}
