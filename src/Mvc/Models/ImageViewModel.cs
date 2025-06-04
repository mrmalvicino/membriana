using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La URL es obligatoria.")]
        [Display(Name = "URL")]
        public string Url { get; set; } = null!;
    }
}
