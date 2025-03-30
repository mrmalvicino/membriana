using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using Mvc.Filters;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mvc.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IUserService _userService;

        public MemberController(IMemberRepository memberRepository, IMembershipPlanRepository membershipPlanRepository, IUserService userService)
        {
            _memberRepository = memberRepository;
            _membershipPlanRepository = membershipPlanRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            int organizationId = await _userService.GetOrganizationId();
            var members = await _memberRepository.GetAllAsync(organizationId);
            return View(members);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);

            if (member == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != member.OrganizationId)
            {
                return NotFound();
            }

            member.MembershipPlan = await _membershipPlanRepository.GetByIdAsync(member.MembershipPlanId);

            return View(member);
        }

        public async Task<IActionResult> Create()
        {
            int organizationId = await _userService.GetOrganizationId();
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);
            ViewBag.MembershipPlanId = new SelectList(membershipPlans, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.OrganizationId = await _userService.GetOrganizationId();
                member = await _memberRepository.AddAsync(member);
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpGet]
        [Route("socios/editar/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);

            if (member == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != member.OrganizationId)
            {
                return NotFound();
            }

            int organizationId = await _userService.GetOrganizationId();
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);
            ViewBag.MembershipPlanId = new SelectList(membershipPlans, "Id", "Name");
            return View(member);
        }

        [HttpPost]
        [Route("socios/editar/{id}")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(TenancyWriteFilter<Member, IMemberRepository>))]
        public async Task<IActionResult> Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                member.OrganizationId = await _userService.GetOrganizationId();
                member = await _memberRepository.UpdateAsync(member);
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);

            if (member == null)
            {
                return NotFound();
            }

            if (await _userService.GetOrganizationId() != member.OrganizationId)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
