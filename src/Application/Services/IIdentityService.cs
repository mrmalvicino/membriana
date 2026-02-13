using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    /// <summary>
    /// Servicio que encapsula las operaciones de <see cref="Microsoft.AspNetCore.Identity"/>
    /// relacionadas con la gestión de usuarios y roles.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Obtiene un usuario a partir de su correo electrónico.
        /// </summary>
        Task<AppUser?> FindByEmail(string email);

        /// <summary>
        /// Valida si la contraseña proporcionada es correcta para un usuario dado.
        /// </summary>
        Task<bool> PasswordIsValid(AppUser user, string password);

        /// <summary>
        /// Crea un nuevo usuario con una contraseña específica.
        /// </summary>
        Task<IdentityResult> CreateUser(AppUser user, string password);

        /// <summary>
        /// Le asigna un rol a un usuario.
        /// </summary>
        Task<IdentityResult> AddToRole(AppUser user, AppRole role);
    }
}
