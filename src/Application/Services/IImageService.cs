using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Administra la imagen de perfil asociada a una persona.
/// </summary>
public interface IImageService
{
	/// <summary>
	/// Prepara la creación, actualización o eliminación de la imagen para que se
	/// persista junto con la entidad propietaria.
	/// </summary>
	void ApplyProfileImage(Person person, string? imageUrl);

	/// <summary>
	/// Prepara la eliminación de la imagen asociada a una persona.
	/// </summary>
	void RemoveProfileImage(Person person);
}
