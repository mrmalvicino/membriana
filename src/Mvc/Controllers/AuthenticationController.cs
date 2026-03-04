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

            if (registerResponseDto is null || string.IsNullOrWhiteSpace(registerResponseDto.Token))
            {
                ModelState.AddModelError("", "No se recibió un token válido.");
                return View(registerViewModel);
            }

            _cookieService.SetJwtCookie(registerResponseDto.Token);

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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
        return View();
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

        // Si querés que el email quede disponible para siguientes requests:
        TempData.Keep("RegisteredEmail");

        return View(registerConfirmation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendConfirmation(string email)
    {
        try
        {
            // llamás a tu AuthApiService
            await _authApiService.ResendConfirmationAsync(email);

            TempData["RegisteredEmail"] = email;
            TempData["ResendOk"] = "Te reenviamos el correo de confirmación. Revisá tu casilla.";
        }
        catch
        {
            TempData["RegisteredEmail"] = email;
            TempData["ResendError"] = "No pudimos reenviar el correo. Intentá nuevamente.";
        }

        return RedirectToAction(nameof(RegisterConfirmation));
    }
}
