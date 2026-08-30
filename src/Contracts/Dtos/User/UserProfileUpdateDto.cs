using System.ComponentModel.DataAnnotations;

namespace Contracts.Dtos.User;

public class UserProfileUpdateDto
{
	[Required]
	[StringLength(150)]
	public string Name { get; set; } = null!;

	[Required]
	[EmailAddress]
	[StringLength(254)]
	public string UserEmail { get; set; } = null!;

	[StringLength(50)]
	public string? Phone { get; set; }

	[StringLength(50)]
	public string? Dni { get; set; }

	[Required]
	public DateTime? BirthDate { get; set; }

	[Url]
	[StringLength(2048)]
	public string? ProfileImageUrl { get; set; }
}
