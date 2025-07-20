using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services
{
    public interface IUserService
    {
        Task<int> GetOrganizationIdAsync();
        Task<JwtSecurityToken> GenerateTokenAsync(AppUser user);
    }
}
