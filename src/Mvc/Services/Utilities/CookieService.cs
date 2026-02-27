using Mvc.Services.Utilities.Interfaces;

namespace Mvc.Services.Utilities;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetJwtCookie(string token)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            "jwt",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            }
        );
    }

    public void DeleteJwtCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt");
    }
}
