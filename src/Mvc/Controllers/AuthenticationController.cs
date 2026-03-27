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

    [HttpPost]
    public IActionResult Logout()
    {
        _cookieService.DeleteJwtCookie();
        return RedirectToAction("Login", "Authentication");
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

            TempData["EmailSendTo"] = registerResponseDto.UserEmail;
            TempData["EmailSendIsSuccess"] = true;
            TempData["EmailSendMessage"] = registerResponseDto.Message;

            return RedirectToAction(nameof(RegisterConfirmation));
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(registerViewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendConfirmation(
        ResendConfirmationViewModel resendConfirmationViewModel
    )
    {
        TempData["EmailSendTo"] = resendConfirmationViewModel.Email;

        try
        {
            var response = await _authenticationApi.ResendConfirmationAsync(resendConfirmationViewModel);

            TempData["EmailSendIsSuccess"] = true;
            TempData["EmailSendMessage"] = response?.Message ?? "Te reenviamos el correo de confirmación.";
        }
        catch (Exception ex)
        {
            TempData["EmailSendIsSuccess"] = false;
            TempData["EmailSendMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(RegisterConfirmation));
    }

    [HttpGet]
    public IActionResult RegisterConfirmation()
    {
        var registerConfirmation = new RegisterConfirmationViewModel
        {
            Email = TempData["EmailSendTo"] as string ?? string.Empty,
            IsSuccess = TempData["EmailSendIsSuccess"] as bool? ?? false,
            Message = TempData["EmailSendMessage"] as string ?? string.Empty
        };

        TempData.Keep("EmailSendTo");
        TempData.Keep("EmailSendIsSuccess");
        TempData.Keep("EmailSendMessage");

        return View(registerConfirmation);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return View(
                new ResponseViewModel
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
                new ResponseViewModel
                {
                    IsSuccess = true,
                    Message = response.Message
                }
            );
        }
        catch (Exception ex)
        {
            return View(
                new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                }
            );
        }
    }
}
