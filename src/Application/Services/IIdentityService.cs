using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IIdentityService
    {
        Task<AppUser?> FindByEmail(string email);

        Task<bool> PasswordIsValid(AppUser user, string password);

        Task<IdentityResult> CreateUser(AppUser user, string password);

        Task<IdentityResult> AddToRole(AppUser user, AppRole role);
    }
}
