using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.Filters;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Controllers
{
    [JwtAuthorizationFilter]
    public class MemberController : Controller
    {
        private readonly IMemberApiService _memberApi;
        private readonly IMembershipPlanApiService _membershipPlanApi;
        private readonly IUserApiService _userApi;

        public MemberController(
            IMemberApiService memberService,
            IMembershipPlanApiService membershipPlanService,
            IUserApiService userService
        )
        {
            _memberApi = memberService;
            _membershipPlanApi = membershipPlanService;
            _userApi = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var organizationId = await _userApi.GetOrganizationIdAsync();
            var members = await _memberApi.GetAllAsync(organizationId);
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberApi.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            if (member.OrganizationId != await _userApi.GetOrganizationIdAsync())
            {
                return NotFound();
            }

            member.MembershipPlan = await _membershipPlanApi.GetByIdAsync(member.MembershipPlanId);

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userOrgId = await _userApi.GetOrganizationIdAsync();
            await SetViewBagMembershipPlans(userOrgId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberViewModel member)
        {
            if (ModelState.IsValid)
            {
                member.OrganizationId = await _userApi.GetOrganizationIdAsync();
                await _memberApi.CreateAsync(member);
                return RedirectToAction(nameof(Index));
            }

            var userOrgId = await _userApi.GetOrganizationIdAsync();
            await SetViewBagMembershipPlans(userOrgId);

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberApi.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            if (member.OrganizationId != await _userApi.GetOrganizationIdAsync())
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
            member.OrganizationId = await _userApi.GetOrganizationIdAsync();

            if (ModelState.IsValid)
            {
                await _memberApi.UpdateAsync(member);
                return RedirectToAction(nameof(Index));
            }

            await SetViewBagMembershipPlans(member.OrganizationId, member.MembershipPlan?.Id);

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberApi.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            if (member.OrganizationId != await _userApi.GetOrganizationIdAsync())
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberApi.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task SetViewBagMembershipPlans(int organizationId, int? selectedPlanId = null)
        {
            var membershipPlans = await _membershipPlanApi.GetAllAsync(organizationId);
            ViewBag.MembershipPlans = new SelectList(membershipPlans, "Id", "Name", selectedPlanId);
        }
    }
}
