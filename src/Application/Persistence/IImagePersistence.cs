using Domain.Entities;

namespace Application.Persistence;

/// <summary>
/// Prepara cambios de persistencia de imágenes para su confirmación posterior.
/// </summary>
public interface IImagePersistence
{
	/// <summary>
	/// Prepara una imagen para su creación.
	/// </summary>
	void StageCreation(Image image);

	/// <summary>
	/// Prepara una imagen para su eliminación.
	/// </summary>
	void StageDeletion(Image image);
}
