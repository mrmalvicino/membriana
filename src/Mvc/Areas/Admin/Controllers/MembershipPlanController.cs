using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Exceptions;
using Mvc.Filters;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class MembershipPlanController : Controller
{
    private readonly IMembershipPlanApiService _membershipPlanApi;
    private readonly IUserApiService _userApi;

    public MembershipPlanController(IMembershipPlanApiService membershipPlanService, IUserApiService userService)
    {
        _membershipPlanApi = membershipPlanService;
        _userApi = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int organizationId = await _userApi.GetOrganizationIdAsync();
        var membershipPlans = await _membershipPlanApi.GetAllAsync(organizationId);
        return View(membershipPlans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanApi.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(membershipPlan);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipPlanViewModel membershipPlan)
    {
        if (ModelState.IsValid)
        {
            membershipPlan.OrganizationId = await _userApi.GetOrganizationIdAsync();
            await _membershipPlanApi.CreateAsync(membershipPlan);
            return RedirectToAction(nameof(Index));
        }

        return View(membershipPlan);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanApi.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(membershipPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MembershipPlanViewModel membershipPlan)
    {
        if (ModelState.IsValid)
        {
            membershipPlan.OrganizationId = await _userApi.GetOrganizationIdAsync();
            await _membershipPlanApi.UpdateAsync(membershipPlan);
            return RedirectToAction(nameof(Index));
        }

        return View(membershipPlan);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanApi.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(membershipPlan);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _membershipPlanApi.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            TempData["BusinessError"] = ex.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
