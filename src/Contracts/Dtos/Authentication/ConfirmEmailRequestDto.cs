namespace Contracts.Dtos.Authentication;

public class ConfirmEmailRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
