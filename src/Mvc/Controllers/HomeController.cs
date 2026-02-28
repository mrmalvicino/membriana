using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers;

/// <summary>
/// Controlador principal para las páginas públicas de la aplicación.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Muestra la LandingPage.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Muestra la página de política de privacidad.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }
}