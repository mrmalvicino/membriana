using Application.Persistence;
using Application.Services;
using Domain.Entities;

namespace Infrastructure.Services;

/// <summary>
/// Administra la persistencia de las imágenes vinculadas a entidades del dominio.
/// </summary>
public class ImageService : IImageService
{
	private readonly IImagePersistence _imagePersistence;

	public ImageService(IImagePersistence imagePersistence)
	{
		_imagePersistence = imagePersistence;
	}

	/// <inheritdoc />
	public void ApplyProfileImage(Person person, string? imageUrl)
	{
		var normalizedUrl = string.IsNullOrWhiteSpace(imageUrl)
			? null
			: imageUrl.Trim();

		if (normalizedUrl == null)
		{
			RemoveProfileImage(person);
			return;
		}

		if (person.ProfileImage == null)
		{
			var profileImage = new Image
			{
				Url = normalizedUrl
			};

			person.ProfileImage = profileImage;
			_imagePersistence.StageCreation(profileImage);
			return;
		}

		person.ProfileImage.Url = normalizedUrl;
	}

	/// <inheritdoc />
	public void RemoveProfileImage(Person person)
	{
		var profileImage = person.ProfileImage;

		if (profileImage == null)
		{
			return;
		}

		person.ProfileImage = null;
		person.ProfileImageId = null;
		_imagePersistence.StageDeletion(profileImage);
	}
}
