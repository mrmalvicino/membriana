using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class MembershipPlanController : AdminControllerBase
{
    private readonly IMembershipPlanClient _membershipPlanClient;
    private readonly IUserClient _userClient;

    public MembershipPlanController(IMembershipPlanClient membershipPlanClient, IUserClient userClient)
    {
        _membershipPlanClient = membershipPlanClient;
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int organizationId = await _userClient.GetOrganizationIdAsync();
        var membershipPlans = await _membershipPlanClient.GetAllAsync(organizationId);
        return View(membershipPlans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanClient.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userClient.GetOrganizationIdAsync())
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
            try
            {
                membershipPlan.OrganizationId = await _userClient.GetOrganizationIdAsync();
                await _membershipPlanClient.CreateAsync(membershipPlan);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (TryAddModelError(ex))
            {
            }
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

        var membershipPlan = await _membershipPlanClient.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userClient.GetOrganizationIdAsync())
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
            try
            {
                membershipPlan.OrganizationId = await _userClient.GetOrganizationIdAsync();
                await _membershipPlanClient.UpdateAsync(membershipPlan);
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

        return View(membershipPlan);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var membershipPlan = await _membershipPlanClient.GetByIdAsync(id.Value);

        if (membershipPlan == null)
        {
            return NotFound();
        }

        if (membershipPlan.OrganizationId != await _userClient.GetOrganizationIdAsync())
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
            await _membershipPlanClient.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (TrySetDeleteError(ex))
        {
            return RedirectToAction(nameof(Delete), new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
