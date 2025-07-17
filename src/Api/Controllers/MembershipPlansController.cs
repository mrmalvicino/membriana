using Api.Filters;
using Application.Dtos.MembershipPlan;
using Application.Repositories;
using Application.Services;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MembershipPlansController(
            IMembershipPlanRepository membershipPlanRepository,
            IUserService userService,
            IMapper mapper
        )
        {
            _membershipPlanRepository = membershipPlanRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(TenancyQueryFilter))]
        public async Task<ActionResult<IEnumerable<MembershipPlanReadDto>>> GetAll(
            [FromQuery] int organizationId
        )
        {
            var membershipPlans = await _membershipPlanRepository.GetAllAsync(organizationId);
            var readDtos = _mapper.Map<IEnumerable<MembershipPlanReadDto>>(membershipPlans);
            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
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
            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (createDto.OrganizationId != userOrgId)
            {
                return Forbid();
            }

            var membershipPlan = _mapper.Map<MembershipPlan>(createDto);
            var created = await _membershipPlanRepository.AddAsync(membershipPlan);
            var readDto = _mapper.Map<MembershipPlanReadDto>(created);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
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
        [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _membershipPlanRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
