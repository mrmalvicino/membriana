using System.ComponentModel.DataAnnotations;

namespace Mvc.Areas.Admin.ViewModels;

public class ImageViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La URL es obligatoria.")]
    [Display(Name = "URL")]
    public string Url { get; set; } = null!;
}
