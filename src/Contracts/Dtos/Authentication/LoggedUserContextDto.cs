namespace Contracts.Dtos.Authentication;

public class LoggedUserContextDto
{
    public string UserId { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; } = null!;
}
