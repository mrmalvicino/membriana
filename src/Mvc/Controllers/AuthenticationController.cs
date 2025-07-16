using Microsoft.AspNetCore.Mvc;
using Mvc.Dtos.Authentication;
using Mvc.Models;
using System.Text;
using System.Text.Json;

namespace Mvc.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public AuthenticationController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8, "application/json"
            );

            var url = $"{_apiBaseUrl}api/authentication/login";
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponseDto>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.Token))
            {
                ModelState.AddModelError("", "No se recibió un token válido");
                return View(model);
            }

            Response.Cookies.Append("jwt", tokenResponse.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                }
            );

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8, "application/json"
            );

            var url = $"{_apiBaseUrl}api/authentication/register";
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponseDto>(
                        errorContent,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                    if (errorResponse != null && errorResponse.Errors != null)
                    {
                        foreach (var error in errorResponse.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Datos inválidos");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error al procesar la respuesta del servidor.");
                }

                return View(model);
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Authentication");
        }
    }
}
