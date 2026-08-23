using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Portal.ViewModels;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;

namespace Mvc.Areas.Portal.Controllers;

/// <summary>
/// Controlador principal para el Dashboard del Client Side (Portal).
/// </summary>
[Area("Portal")]
[JwtAuthorizationFilter]
public class DashboardController : Controller
{
    private readonly IPaymentClient _paymentClient;
    private readonly IUserClient _userClient;
    private readonly IMemberClient _memberClient;

    public DashboardController(
        IPaymentClient paymentClient,
        IUserClient userClient,
        IMemberClient memberClient
    )
    {
        _paymentClient = paymentClient;
        _userClient = userClient;
        _memberClient = memberClient;
    }

    /// <summary>
    /// Muestra el Dashboard.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var loggedUserContext = await _userClient.GetLoggedUserContextAsync();
        var loggedMember = await _memberClient.GetForLoggedUserAsync();
        var payments = await _paymentClient.GetAllForLoggedUser();

        var lastPayment = payments
            .OrderByDescending(payment => payment.DateTime)
            .FirstOrDefault();

        if (!loggedUserContext.MemberId.HasValue)
        {
            return Forbid();
        }

        if (loggedMember == null)
        {
            return NotFound();
        }

		var dashboard = new DashboardViewModel
		{
			MemberFullName = loggedMember.Name,
			ProfileImageUrl = loggedMember.ProfileImage?.Url,
			Document = loggedMember.Dni,
            Email = loggedMember.Email,
            Phone = loggedMember.Phone,

            OrganizationName = loggedUserContext.OrganizationName,
            MembershipPlanName = loggedMember.MembershipPlan?.Name ?? string.Empty,

            AdmissionDate = loggedMember.AdmissionDate,
            Status = loggedMember.MemberStatus,

            LastPaymentAmount = lastPayment?.Amount,
            LastPaymentDate = lastPayment?.DateTime
        };

        return View(dashboard);
    }
}
