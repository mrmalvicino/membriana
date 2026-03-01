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
    /// Muestra la página de licencia del software.
    /// </summary>
    public IActionResult License()
    {
        return View();
    }

    /// <summary>
    /// Muestra la página de políticas de privacidad.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Muestra la página de términos y condiciones de uso.
    /// </summary>
    public IActionResult Terms()
    {
        return View();
    }
}