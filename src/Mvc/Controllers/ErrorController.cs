using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using System.Diagnostics;

public class ErrorController : Controller
{
    [Route("Error")]
    public IActionResult Index()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var vm = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Path = feature?.Path,
        };

        Response.StatusCode = 500;
        return View("Error", vm);
    }

    [Route("Error/{statusCode:int}")]
    public IActionResult StatusCode(int statusCode)
    {
        var vm = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = statusCode
        };

        return statusCode switch
        {
            404 => View("NotFound", vm),
            403 => View("Forbidden", vm),
            _ => View("StatusCode", vm)
        };
    }
}
