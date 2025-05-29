using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Repositories;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId)
        {
            var members = await _memberRepository.GetAllAsync(organizationId);
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Member member)
        {
            var created = await _memberRepository.AddAsync(member);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            var updated = await _memberRepository.UpdateAsync(member);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
