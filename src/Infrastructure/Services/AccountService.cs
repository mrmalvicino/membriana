using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Infrastructure.Services;

/// <summary>
/// Servicio de aplicación encargado de gestionar el ciclo de vida de las cuentas de usuario.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    
    public AccountService(
        IIdentityService identityService,
        IEmailService emailService
    )
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    /// <summary>
    /// Envía un correo de confirmación de cuenta al usuario especificado.
    /// </summary>
    public async Task SendConfirmationAsync(AppUser user)
    {
        var token = await _identityService.GenerateEmailConfirmationToken(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmationLink = $"{_frontendUrl}/auth/confirm?userId={user.Id}&token={encodedToken}";

        await _emailService.SendAsync(
            user.Email!,
            "Confirmá tu cuenta",
            $"Hacé click acá para confirmar tu cuenta: <a href='{confirmationLink}'>Confirmar</a>"
        );
    }
}
