using Microsoft.AspNetCore.Mvc;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;
using Mvc.Controllers;

namespace Mvc.Areas.Portal.Controllers;

[Area("Portal")]
[JwtAuthorizationFilter]
public class PaymentController : MvcControllerBase
{
    private readonly IPaymentClient _paymentClient;
    private readonly IUserClient _userClient;

    public PaymentController(
        IPaymentClient paymentClient,
        IUserClient userClient
    )
    {
        _paymentClient = paymentClient;
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var loggedUserContext = await _userClient.GetLoggedUserContextAsync();

        if (!loggedUserContext.MemberId.HasValue)
        {
            return Forbid();
        }

        var payments = await _paymentClient.GetAllByMemberIdAsync(
            loggedUserContext.MemberId.Value
        );

        return View(payments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _paymentClient.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        if (payment.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(payment);
    }
}
