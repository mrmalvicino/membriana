using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Infrastructure.Dtos.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;

namespace Infrastructure.Services;

/// <summary>
/// Servicio de aplicación encargado de gestionar el ciclo de vida de las cuentas de usuario.
/// </summary>
public class AccountService : IAccountService
{
    private readonly MembrianaSettingsDto _membrianaSettings;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly IOrganizationRepository _organizationRepository;

    public AccountService(
        IOptions<MembrianaSettingsDto> membrianaSettings,
        IIdentityService identityService,
        IEmailService emailService,
        IOrganizationRepository organizationRepository
    )
    {
        _membrianaSettings = membrianaSettings.Value;
        _identityService = identityService;
        _emailService = emailService;
        _organizationRepository = organizationRepository;
    }

    /// <summary>
    /// Envía un correo de confirmación de cuenta al usuario especificado.
    /// </summary>
    public async Task SendConfirmationAsync(AppUser user)
    {
        var token = await _identityService.GenerateEmailConfirmationToken(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmationLink =
            $"{_membrianaSettings.HostingUrl}/Authentication/ConfirmEmail?userId={user.Id}&token={encodedToken}";

        var organization = await _organizationRepository.GetByIdAsync(user.OrganizationId);

        await _emailService.SendConfirmationEmailAsync(
            user.Email,
            organization.Name,
            confirmationLink
        );
    }
}
