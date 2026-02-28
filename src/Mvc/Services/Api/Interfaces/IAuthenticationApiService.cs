using Contracts.Dtos.Authentication;
using Mvc.ViewModels;

namespace Mvc.Services.Api.Interfaces;

public interface IAuthenticationApiService
{
    Task<LoginResponseDto?> LoginAsync(LoginViewModel loginViewModel);
    Task<RegisterResponseDto?> RegisterAsync(RegisterViewModel registerViewModel);
}
