using Application.Dtos.MembershipPlan;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlansController : ControllerBase
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IMapper _mapper;

        public MembershipPlansController(
            IMembershipPlanRepository membershipPlanRepository,
            IMapper mapper
        )
        {
            _membershipPlanRepository = membershipPlanRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<MembershipPlanReadDto>>> GetAll(
            [FromQuery] int organizationId
        )
        {
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);

            if (!membershipPlans.Any())
            {
                return NoContent();
            }

            var readDtos = _mapper.Map<IEnumerable<MembershipPlanReadDto>>(membershipPlans);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipPlanReadDto>> Get(int id)
        {
            var membershipPlan = await _membershipPlanRepository.GetByIdAsync(id);

            if (membershipPlan == null)
            {
                return NotFound();
            }

            var readDto = _mapper.Map<MembershipPlanReadDto>(membershipPlan);

            return Ok(readDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MembershipPlanReadDto>> Create(
            [FromBody] MembershipPlanCreateDto createDto
        )
        {
            var membershipPlan = _mapper.Map<MembershipPlan>(createDto);
            var created = await _membershipPlanRepository.AddAsync(membershipPlan);
            var readDto = _mapper.Map<MembershipPlanReadDto>(created);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MembershipPlanReadDto>> Update(
            int id,
            [FromBody] MembershipPlanUpdateDto updateDto
        )
        {
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            var membershipPlan = _mapper.Map<MembershipPlan>(updateDto);
            var updated = await _membershipPlanRepository.UpdateAsync(membershipPlan);
            var readDto = _mapper.Map<MembershipPlanReadDto>(updated);

            return Ok(readDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _membershipPlanRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
