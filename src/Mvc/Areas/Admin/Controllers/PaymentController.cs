using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Filters;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class PaymentController : Controller
{
    private readonly IPaymentApiService _paymentApi;
    private readonly IMemberApiService _memberApi;
    private readonly IUserApiService _userApi;

    public PaymentController(
        IPaymentApiService paymentApi,
        IMemberApiService memberApi,
        IUserApiService userApi
    )
    {
        _paymentApi = paymentApi;
        _memberApi = memberApi;
        _userApi = userApi;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizationId = await _userApi.GetOrganizationIdAsync();
        var payments = await _paymentApi.GetAllAsync(organizationId);
        return View(payments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _paymentApi.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        if (payment.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(payment);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var organizationId = await _userApi.GetOrganizationIdAsync();
        await SetViewBagMembers(organizationId);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentViewModel payment)
    {
        if (ModelState.IsValid)
        {
            payment.OrganizationId = await _userApi.GetOrganizationIdAsync();
            await _paymentApi.CreateAsync(payment);
            return RedirectToAction(nameof(Index));
        }

        var organizationId = await _userApi.GetOrganizationIdAsync();
        await SetViewBagMembers(organizationId);
        return View(payment);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _paymentApi.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        if (payment.OrganizationId != await _userApi.GetOrganizationIdAsync())
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
        payment.OrganizationId = await _userApi.GetOrganizationIdAsync();

        if (ModelState.IsValid)
        {
            await _paymentApi.UpdateAsync(payment);
            return RedirectToAction(nameof(Index));
        }

        await SetViewBagMembers(payment.OrganizationId, payment.MemberId);
        return View(payment);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _paymentApi.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        if (payment.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(payment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _paymentApi.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task SetViewBagMembers(int organizationId, int? selectedMemberId = null)
    {
        var members = await _memberApi.GetAllAsync(organizationId);
        ViewBag.Members = new SelectList(members, "Id", "Name", selectedMemberId);
    }
}
