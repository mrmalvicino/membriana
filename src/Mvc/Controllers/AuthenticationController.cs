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

        public AuthenticationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

            var response = await _httpClient.PostAsync(
                "https://localhost:7076/api/authentication/login",
                content
            );

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

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Authentication");
        }
    }
}
