using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using System.Diagnostics;

namespace Mvc.Controllers;

/// <summary>
/// Controlador para manejar mostrar errores.
/// </summary>
public class ErrorController : Controller
{
    /// <summary>
    /// Maneja las excepciones disparadas desde el Middleware Pipeline
    /// por <see cref="IApplicationBuilder.UseExceptionHandler"/>.
    /// </summary>
    [Route("Error")]
    public IActionResult Index()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Path = feature?.Path,
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        Response.StatusCode = StatusCodes.Status500InternalServerError;

        return View("Error", errorViewModel);
    }

    /// <summary>
    /// Maneja los códigos de estado disparados desde el Middleware Pipeline
    /// por <see cref="IApplicationBuilder.UseStatusCodePagesWithReExecute"/>.
    /// </summary>
    [Route("Error/{statusCode:int}")]
    public IActionResult StatusCode(int statusCode)
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = statusCode
        };

        switch (statusCode)
        {
            case StatusCodes.Status404NotFound:
                return View("NotFound", errorViewModel);
            case StatusCodes.Status403Forbidden:
                return View("Forbidden", errorViewModel);
            default:
                return View("StatusCode", errorViewModel);
        }
    }
}
