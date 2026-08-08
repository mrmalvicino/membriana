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

    public PaymentController(IPaymentClient paymentClient)
    {
        _paymentClient = paymentClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var payments = await _paymentClient.GetAllForLoggedUser();
        return View(payments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _paymentClient.GetByIdForLoggedUserAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        return View(payment);
    }
}
