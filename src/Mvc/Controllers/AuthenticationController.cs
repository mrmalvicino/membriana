using Microsoft.AspNetCore.Mvc;
using Mvc.Services.Api.Interfaces;
using Mvc.Services.Utilities.Interfaces;
using Mvc.ViewModels;

namespace Mvc.Controllers;

public class AuthenticationController : Controller
{
    private readonly IAuthenticationApiService _authenticationApi;
    private readonly ICookieService _cookieService;

    public AuthenticationController(
        IAuthenticationApiService authenticationApi,
        ICookieService cookieService
    )
    {
        _authenticationApi = authenticationApi;
        _cookieService = cookieService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var loginResponse = await _authenticationApi.LoginAsync(loginViewModel);

        if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            ModelState.AddModelError("", "Credenciales inválidas.");
            return View(loginViewModel);
        }

        _cookieService.SetJwtCookie(loginResponse.Token);

        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }

        try
        {
            var registerResponseDto = await _authenticationApi.RegisterAsync(registerViewModel);

            if (registerResponseDto is null)
            {
                ModelState.AddModelError("", "No se recibió una respuesta válida.");
                return View(registerViewModel);
            }

            TempData["RegisteredEmail"] = registerResponseDto.UserEmail;

            return RedirectToAction(nameof(RegisterConfirmation));
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(registerViewModel);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _cookieService.DeleteJwtCookie();
        return RedirectToAction("Login", "Authentication");
    }

    [HttpGet]
    public IActionResult RegisterConfirmation()
    {
        var registerConfirmation = new RegisterConfirmationViewModel
        {
            Email = TempData["RegisteredEmail"] as string ?? string.Empty,
            SuccessMessage = TempData["ResendOk"] as string,
            ErrorMessage = TempData["ResendError"] as string
        };

        TempData.Keep("RegisteredEmail");

        return View(registerConfirmation);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return View(
                new ConfirmEmailViewModel
                {
                    IsSuccess = false,
                    Message = "El enlace de confirmación es inválido o está incompleto."
                }
            );
        }

        try
        {
            var response = await _authenticationApi.ConfirmEmailAsync(
                new ConfirmEmailViewModel
                {
                    UserId = userId,
                    Token = token
                }
            );

            return View(
                new ConfirmEmailViewModel
                {
                    IsSuccess = true,
                    Message = response.Message
                }
            );
        }
        catch (ApplicationException ex)
        {
            return View(
                new ConfirmEmailViewModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                }
            );
        }
        catch
        {
            return View(
                new ConfirmEmailViewModel
                {
                    IsSuccess = false,
                    Message = "No pudimos confirmar tu email en este momento. Intentá nuevamente."
                }
            );
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendConfirmation(
        ResendConfirmationViewModel resendConfirmationViewModel
    )
    {
        TempData["RegisteredEmail"] = resendConfirmationViewModel.Email;

        try
        {
            var response = await _authenticationApi.ResendConfirmationAsync(resendConfirmationViewModel);

            TempData["ResendOk"] =
                response?.Message ?? "Te reenviamos el correo de confirmación. Revisá tu casilla.";
        }
        catch (ApplicationException ex)
        {
            TempData["ResendError"] = ex.Message;
        }
        catch
        {
            TempData["ResendError"] = "No pudimos reenviar el correo. Intentá nuevamente.";
        }

        return RedirectToAction(nameof(RegisterConfirmation));
    }
}
