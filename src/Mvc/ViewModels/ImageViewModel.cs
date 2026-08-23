using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels;

public class ImageViewModel
{
    public int Id { get; set; }

	[Display(Name = "URL de la imagen")]
	[Url(ErrorMessage = "La URL de la imagen no tiene un formato válido.")]
	[StringLength(2048, ErrorMessage = "La URL de la imagen no puede superar los 2048 caracteres.")]
	public string? Url { get; set; }
}
