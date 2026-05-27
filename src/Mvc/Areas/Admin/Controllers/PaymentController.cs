using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class PaymentController : AdminControllerBase
{
    private readonly IPaymentClient _paymentClient;
    private readonly IMemberClient _memberClient;
    private readonly IUserClient _userClient;

    public PaymentController(
        IPaymentClient paymentClient,
        IMemberClient memberClient,
        IUserClient userClient
    )
    {
        _paymentClient = paymentClient;
        _memberClient = memberClient;
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizationId = await _userClient.GetOrganizationIdAsync();
        var payments = await _paymentClient.GetAllAsync(organizationId);
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var organizationId = await _userClient.GetOrganizationIdAsync();
        await SetViewBagMembers(organizationId);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentViewModel payment)
    {
        if (ModelState.IsValid)
        {
            try
            {
                payment.OrganizationId = await _userClient.GetOrganizationIdAsync();
                await _paymentClient.CreateAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (TryAddModelError(ex))
            {
            }
        }

        var organizationId = await _userClient.GetOrganizationIdAsync();
        await SetViewBagMembers(organizationId);
        return View(payment);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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

        await SetViewBagMembers(payment.OrganizationId, payment.MemberId);

        return View(payment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PaymentViewModel payment)
    {
        payment.OrganizationId = await _userClient.GetOrganizationIdAsync();

        if (ModelState.IsValid)
        {
            try
            {
                await _paymentClient.UpdateAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex) when (TryAddModelError(ex))
            {
            }
        }

        await SetViewBagMembers(payment.OrganizationId, payment.MemberId);
        return View(payment);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
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

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _paymentClient.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex) when (TrySetDeleteError(ex))
        {
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    private async Task SetViewBagMembers(int organizationId, int? selectedMemberId = null)
    {
        var members = await _memberClient.GetAllAsync(organizationId);
        ViewBag.Members = new SelectList(members, "Id", "Name", selectedMemberId);
    }
}
