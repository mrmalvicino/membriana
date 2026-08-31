using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels;

public class ProfileViewModel
{
	[Display(Name = "Nombre y apellido")]
	[Required(ErrorMessage = "El nombre es obligatorio.")]
	[StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "El email es obligatorio.")]
	[EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
	[StringLength(254, ErrorMessage = "El email no puede superar los 254 caracteres.")]
	public string UserEmail { get; set; } = null!;

	[Display(Name = "Teléfono")]
	[StringLength(50, ErrorMessage = "El teléfono no puede superar los 50 caracteres.")]
	public string? Phone { get; set; }

	[Display(Name = "Fecha de nacimiento")]
	[Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
	[DataType(DataType.Date)]
	public DateTime? BirthDate { get; set; }

	[Display(Name = "DNI")]
	[StringLength(50, ErrorMessage = "El DNI no puede superar los 50 caracteres.")]
	public string? Dni { get; set; }

	[Display(Name = "URL de la imagen")]
	[Url(ErrorMessage = "La URL de la imagen no tiene un formato válido.")]
	[StringLength(2048, ErrorMessage = "La URL de la imagen no puede superar los 2048 caracteres.")]
	public string? ProfileImageUrl { get; set; }

	public string Role { get; set; } = string.Empty;

	public bool HasAssociatedPerson { get; set; }

	public bool LoadFailed { get; set; }
}
