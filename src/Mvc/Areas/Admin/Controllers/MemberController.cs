using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class MemberController : AdminControllerBase
{
    private readonly IMemberClient _memberClient;
    private readonly IMembershipPlanClient _membershipPlanClient;
    private readonly IUserClient _userClient;

    public MemberController(
        IMemberClient memberClient,
        IMembershipPlanClient membershipPlanClient,
        IUserClient userClient
    )
    {
        _memberClient = memberClient;
        _membershipPlanClient = membershipPlanClient;
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizationId = await _userClient.GetOrganizationIdAsync();
        var members = await _memberClient.GetAllAsync(organizationId);
        return View(members);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var member = await _memberClient.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        if (member.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        member.MembershipPlan = await _membershipPlanClient.GetByIdAsync(member.MembershipPlanId);

        return View(member);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userOrgId = await _userClient.GetOrganizationIdAsync();
        await SetViewBagMembershipPlans(userOrgId);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberViewModel member)
    {
        if (ModelState.IsValid)
        {
            try
            {
                member.OrganizationId = await _userClient.GetOrganizationIdAsync();
                await _memberClient.CreateAsync(member);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (TryAddModelError(ex))
            {
            }
        }

        var userOrgId = await _userClient.GetOrganizationIdAsync();
        await SetViewBagMembershipPlans(userOrgId);

        return View(member);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var member = await _memberClient.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        if (member.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        await SetViewBagMembershipPlans(member.OrganizationId, member.MembershipPlan?.Id);

        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberViewModel member)
    {
        member.OrganizationId = await _userClient.GetOrganizationIdAsync();

        if (ModelState.IsValid)
        {
            try
            {
                await _memberClient.UpdateAsync(member);
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

        await SetViewBagMembershipPlans(member.OrganizationId, member.MembershipPlan?.Id);

        return View(member);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _memberClient.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        if (member.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(member);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _memberClient.DeleteAsync(id);
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

    private async Task SetViewBagMembershipPlans(int organizationId, int? selectedPlanId = null)
    {
        var membershipPlans = await _membershipPlanClient.GetAllAsync(organizationId);
        ViewBag.MembershipPlans = new SelectList(membershipPlans, "Id", "Name", selectedPlanId);
    }
}
