namespace Contracts.Dtos.User;

public class UserCandidateDto
{
    public int Id { get; set; }
    public string ReferenceCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
