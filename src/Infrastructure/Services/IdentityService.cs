using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    /// <summary>
    /// Servicio que encapsula las operaciones de <see cref="Microsoft.AspNetCore.Identity"/>
    /// relacionadas con la gestión de usuarios y roles.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Obtiene un usuario a partir de su correo electrónico.
        /// </summary>
        public async Task<AppUser?> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Valida si la contraseña proporcionada es correcta para un usuario dado.
        /// </summary>
        public async Task<bool> PasswordIsValid(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Crea un nuevo usuario con una contraseña específica.
        /// </summary>
        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        
        /// <summary>
        /// Le asigna un rol a un usuario.
        /// </summary>
        public async Task<IdentityResult> AddToRole(AppUser user, AppRole role)
        {
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }
    }
}
