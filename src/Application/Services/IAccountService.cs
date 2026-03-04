using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Servicio de aplicación encargado de gestionar el ciclo de vida de las cuentas de usuario.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Envía un correo de confirmación de cuenta al usuario especificado.
    /// </summary>
    Task SendConfirmationAsync(AppUser user);
}
