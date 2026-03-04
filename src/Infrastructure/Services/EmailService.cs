using Application.Services;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendAsync(string to, string subject, string htmlBody)
    {
        throw new NotImplementedException();
    }
}
