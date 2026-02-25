using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// Servicio para gestionar el acceso a información del usuario en sesión
    /// y a la generación de tokens de autenticación.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        public UserService(
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene el usuario actualmente autenticado en el contexto HTTP actual.
        /// </summary>
        /// <remarks>
        /// Cuando un usuario autenticado realiza una petición HTTP al servidor, el
        /// middleware de autenticación valida el token JWT (extraído desde la cookie
        /// o header Authorization) y, si es válido, construye un objeto ClaimsPrincipal
        /// con los datos del usuario mediante <see cref="IHttpContextAccessor"/> y
        /// utiliza <see cref="UserManager{TUser}"/> para recuperar desde la base de datos
        /// la entidad <see cref="AppUser"/> completa.
        /// </remarks>
        public async Task<AppUser> GetLoggedUserAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                throw new Exception("No hay usuario en sesión.");
            }

            return user;
        }

        /// <summary>
        /// Obtiene el ID de la organización (tenant) del usuario autenticado en el request actual.
        /// </summary>
        /// <remarks>
        /// Este método es utilizado por controladores y servicios de aplicación para
        /// validar y reforzar el aislamiento multi-tenant, evitando que un usuario
        /// opere sobre datos de una organización distinta a la propia.
        /// </remarks>
        public async Task<int> GetOrganizationIdAsync()
        {
            var user = await GetLoggedUserAsync();

            if (user.OrganizationId == 0)
            {
                throw new Exception("No se encontró la organización del usuario.");
            }

            return user.OrganizationId;
        }

        /// <summary>
        /// Genera un token JWT firmado para el usuario indicado, incluyendo claims de
        /// identidad, roles y tenant.
        /// </summary>
        /// <remarks>
        /// Este método suele ser invocado desde flujos de login o emisión de credenciales,
        /// mientras que el consumo del token queda a cargo de los middlewares de
        /// autenticación configurados en la aplicación.
        /// </remarks>
        public async Task<JwtSecurityToken> GenerateTokenAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("OrganizationId", user.OrganizationId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
