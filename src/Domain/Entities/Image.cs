using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Image : IIdentifiable
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = null!;
    }
}
