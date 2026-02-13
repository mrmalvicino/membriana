using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services
{
    /// <summary>
    /// Servicio para gestionar el acceso a información del usuario en sesión
    /// y a la generación de tokens de autenticación.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtiene el ID de la organización (tenant) del usuario autenticado en el request actual.
        /// </summary>
        /// <remarks>
        /// Este método es utilizado por controladores y servicios de aplicación para
        /// validar y reforzar el aislamiento multi-tenant, evitando que un usuario
        /// opere sobre datos de una organización distinta a la propia.
        /// </remarks>
        Task<int> GetOrganizationIdAsync();

        /// <summary>
        /// Genera un token JWT firmado para el usuario indicado, incluyendo claims de
        /// identidad, roles y tenant.
        /// </summary>
        /// <remarks>
        /// Este método suele ser invocado desde flujos de login o emisión de credenciales,
        /// mientras que el consumo del token queda a cargo de los middlewares de
        /// autenticación configurados en la aplicación.
        /// </remarks>
        Task<JwtSecurityToken> GenerateTokenAsync(AppUser user);
    }
}
