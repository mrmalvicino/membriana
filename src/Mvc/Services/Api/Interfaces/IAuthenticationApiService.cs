using Contracts.Dtos.Authentication;
using Mvc.Models;

namespace Mvc.Services.Api.Interfaces;

public interface IAuthenticationApiService
{
    Task<LoginResponseDto?> LoginAsync(LoginViewModel loginViewModel);
    Task<RegisterResponseDto?> RegisterAsync(RegisterViewModel registerViewModel);
}
