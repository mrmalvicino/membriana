using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mvc.Filters;

public class JwtAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasToken = context.HttpContext.Request.Cookies.ContainsKey("jwt");

        if (!hasToken)
        {
            context.Result = new RedirectToActionResult("Login", "Authentication", null);
        }
    }
}
