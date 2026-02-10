using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services
{
    /// <summary>
    /// Abstrae el acceso a información del usuario en sesión y a la generación de tokens
    /// de autenticación, desacoplando a las capas superiores (API, MVC, servicios de aplicación)
    /// de los detalles de infraestructura como Identity, HTTP o JWT.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtiene el identificador de la organización (tenant) asociada
        /// al usuario autenticado en el contexto de ejecución actual.
        /// </summary>
        /// <remarks>
        /// Este método es utilizado por controladores y servicios de aplicación para
        /// validar y reforzar el aislamiento multi-tenant, evitando que un usuario
        /// opere sobre datos de una organización distinta a la propia.
        /// </remarks>
        Task<int> GetOrganizationIdAsync();

        /// <summary>
        /// Genera un token de autenticación para el usuario especificado.
        /// </summary>
        /// <remarks>
        /// Este método suele ser invocado desde flujos de login o emisión de credenciales,
        /// mientras que el consumo del token queda a cargo de los middlewares de
        /// autenticación configurados en la aplicación.
        /// </remarks>
        Task<JwtSecurityToken> GenerateTokenAsync(AppUser user);
    }
}
