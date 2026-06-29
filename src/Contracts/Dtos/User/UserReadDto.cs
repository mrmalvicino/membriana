namespace Contracts.Dtos.User;

public class UserReadDto
{
    public string Id { get; set; } = string.Empty;
    public string ReferenceCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? LinkedPersonName { get; set; }
    public string? LinkedPersonType { get; set; }
}
