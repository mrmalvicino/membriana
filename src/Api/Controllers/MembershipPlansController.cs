using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Repositories;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlansController : ControllerBase
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;

        public MembershipPlansController(IMembershipPlanRepository membershipPlanRepository)
        {
            _membershipPlanRepository = membershipPlanRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId)
        {
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);
            return Ok(membershipPlans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var membershipPlan = await _membershipPlanRepository.GetByIdAsync(id);

            if (membershipPlan == null)
            {
                return NotFound();
            }

            return Ok(membershipPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MembershipPlan membershipPlan)
        {
            var created = await _membershipPlanRepository.AddAsync(membershipPlan);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MembershipPlan membershipPlan)
        {
            if (id != membershipPlan.Id)
            {
                return BadRequest();
            }

            var updated = await _membershipPlanRepository.UpdateAsync(membershipPlan);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _membershipPlanRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
