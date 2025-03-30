using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Repositories;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Mvc.Filters;

namespace Mvc.Controllers
{
    [Authorize]
    public class MembershipPlanController : Controller
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IUserService _userService;

        public MembershipPlanController(IMembershipPlanRepository membershipPlanRepository, IUserService userService)
        {
            _membershipPlanRepository = membershipPlanRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            int organizationId = await _userService.GetOrganizationId();
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);
            return View(membershipPlans);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _membershipPlanRepository.GetByIdAsync(id.Value);

            if (membershipPlan == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != membershipPlan.OrganizationId)
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
        public async Task<IActionResult> Create(MembershipPlan membershipPlan)
        {
            if (ModelState.IsValid)
            {
                membershipPlan.OrganizationId = await _userService.GetOrganizationId();
                membershipPlan = await _membershipPlanRepository.AddAsync(membershipPlan);
                return RedirectToAction(nameof(Index));
            }

            return View(membershipPlan);
        }

        [HttpGet]
        [Route("planes/editar/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _membershipPlanRepository.GetByIdAsync(id.Value);

            if (membershipPlan == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != membershipPlan.OrganizationId)
            {
                return NotFound();
            }

            return View(membershipPlan);
        }

        [HttpPost]
        [Route("planes/editar/{id}")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(TenancyWriteFilter<MembershipPlan, IMembershipPlanRepository>))]
        public async Task<IActionResult> Edit(MembershipPlan membershipPlan)
        {
            if (ModelState.IsValid)
            {
                membershipPlan.OrganizationId = await _userService.GetOrganizationId();
                membershipPlan = await _membershipPlanRepository.UpdateAsync(membershipPlan);
                return RedirectToAction(nameof(Index));
            }

            return View(membershipPlan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _membershipPlanRepository.GetByIdAsync(id.Value);

            if (membershipPlan == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != membershipPlan.OrganizationId)
            {
                return NotFound();
            }

            return View(membershipPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _membershipPlanRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
