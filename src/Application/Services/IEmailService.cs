namespace Application.Services;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(
        string userEmail,
        string organizationName,
        string confirmEmailUrl
    );
}