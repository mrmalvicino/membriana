namespace Contracts.Dtos.User;

public class UserProfileReadDto
{
	public string? Name { get; set; }
	public string Email { get; set; } = null!;
	public string? Phone { get; set; }
	public string? Dni { get; set; }
	public DateTime? BirthDate { get; set; }
	public string? ProfileImageUrl { get; set; }
	public bool HasAssociatedPerson { get; set; }
}
