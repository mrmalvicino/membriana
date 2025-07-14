using Api.Filters;
using Application.Dtos.Member;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MembersController(
            IMemberRepository memberRepository,
            IUserService userService,
            IMapper mapper
        )
        {
            _memberRepository = memberRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ServiceFilter(typeof(TenancyQueryFilter))]
        public async Task<ActionResult<IEnumerable<MemberReadDto>>> GetAll(
            [FromQuery] int organizationId
        )
        {
            var members = await _memberRepository.GetAllAsync(organizationId);

            if (!members.Any())
            {
                return NoContent();
            }

            var readDtos = _mapper.Map<IEnumerable<MemberReadDto>>(members);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public async Task<ActionResult<MemberReadDto>> Get(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            var readDto = _mapper.Map<MemberReadDto>(member);

            return Ok(readDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MemberReadDto>> Create(
            [FromBody] MemberCreateDto createDto
        )
        {
            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (createDto.OrganizationId != userOrgId)
            {
                return Forbid();
            }

            var member = _mapper.Map<Member>(createDto);
            var created = await _memberRepository.AddAsync(member);
            var readDto = _mapper.Map<MemberReadDto>(created);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public async Task<ActionResult<MemberReadDto>> Update(
            int id,
            [FromBody] MemberUpdateDto updateDto
        )
        {
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            var member = _mapper.Map<Member>(updateDto);
            var updated = await _memberRepository.UpdateAsync(member);
            var readDto = _mapper.Map<MemberReadDto>(updated);

            return Ok(readDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
