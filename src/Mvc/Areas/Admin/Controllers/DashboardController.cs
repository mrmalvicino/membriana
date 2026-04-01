using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Filters;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

/// <summary>
/// Controlador principal para el Dashboard del Back Office (Admin).
/// </summary>
[Area("Admin")]
[JwtAuthorizationFilter]
public class DashboardController : Controller
{
    private readonly IUserApiService _userApi;

    public DashboardController(IUserApiService userService)
    {
        _userApi = userService;
    }

    /// <summary>
    /// Muestra el Dashboard.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var loggedUserContext = await _userApi.GetLoggedUserContextAsync();

        var dashboard = new DashboardViewModel
        {
            OrganizationName = loggedUserContext.OrganizationName,
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
