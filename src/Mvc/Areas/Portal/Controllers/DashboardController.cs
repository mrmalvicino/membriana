using Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Portal.ViewModels;
using Mvc.Filters;

namespace Mvc.Areas.Portal.Controllers;

/// <summary>
/// Controlador principal para el Dashboard del Client Side (Portal).
/// </summary>
[Area("Portal")]
[JwtAuthorizationFilter]
public class DashboardController : Controller
{
    /// <summary>
    /// Muestra el Dashboard.
    /// </summary>
    public IActionResult Index()
    {
        var dashboard = new DashboardViewModel
        {
            MemberFullName = "Maximiliano Malvicino",
            Document = "DNI 12.345.678",
            Email = "maxi@email.com",
            Phone = "+54 11 5555-5555",

            OrganizationName = "Ecodev Software",
            MembershipPlanName = "Plan Mensual",

            AdmissionDate = new DateTime(2025, 11, 3),
            Status = MemberStatus.Active,

            LastPayment = new PaymentSummaryViewModel
            {
                Amount = 12500m,
                PaidAt = new DateTime(2026, 2, 20)
            }
        };

        return View(dashboard);
    }
}
