using Microsoft.AspNetCore.Mvc;
using Mvc.Filters;
using Mvc.Models;

namespace Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Mock del Dashboard.
    /// </summary>
    [JwtAuthorizationFilter]
    public IActionResult Dashboard()
    {
        var dashboard = new DashboardViewModel
        {
            ActiveMembersCount = 342,
            ActiveMembersVariationPercent = 4.9m,
            InactiveMembersCount = 58,
            InactiveMembersVariationPercent = -2.1m,
            MonthlySignupsCount = 18,
            MonthlySignupsVariationPercent = 20m,
            MonthlyCancellationsCount = 7,
            MonthlyCancellationsVariationPercent = 75m,
            MonthlyIncome = 1_250_000m,
            MonthlyIncomeVariationPercent = -12.3m,
            Months = new() { "Ene", "Feb", "Mar", "Abr", "May", "Jun" },
            ActiveMembersByMonth = new() { 280, 295, 310, 320, 330, 342 },
            SignupsByMonth = new() { 12, 18, 22, 15, 19, 18 },
            CancellationsByMonth = new() { 5, 7, 6, 9, 8, 7 }
        };

        return View(dashboard);
    }
}