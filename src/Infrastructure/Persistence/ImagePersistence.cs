using Application.Persistence;
using Domain.Entities;

namespace Infrastructure.Persistence;

/// <summary>
/// Prepara cambios de imágenes en el contexto de persistencia compartido.
/// </summary>
public class ImagePersistence : IImagePersistence
{
	private readonly AppDbContext _dbContext;

	public ImagePersistence(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <inheritdoc />
	public void StageCreation(Image image)
	{
		_dbContext.Images.Add(image);
	}

	/// <inheritdoc />
	public void StageDeletion(Image image)
	{
		_dbContext.Images.Remove(image);
	}
}
