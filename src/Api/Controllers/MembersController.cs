using Api.Filters;
using Contracts.Dtos.Member;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Policy = "Employee")]
public class MembersController : BaseController<
    Member,
    IMemberRepository,
    MemberReadDto,
    MemberCreateDto,
    MemberUpdateDto
>
{
    private readonly IMemberService _memberService;

    /// <summary>
    /// Constructor principal.
    /// </summary>
    public MembersController(
        IMemberRepository repository,
        IMemberService memberService,
        IUserService userService,
        IMapper mapper
    ) : base(repository, userService, mapper)
    {
        _memberService = memberService;
    }

    public override async Task<ActionResult<MemberReadDto>> Create([FromBody] MemberCreateDto createDto)
    {
        int userOrgId = await _userService.GetOrganizationIdAsync();

        if (createDto.OrganizationId != userOrgId)
        {
            return Forbid();
        }

        var entity = _mapper.Map<Member>(createDto);
        var created = await _memberService.AddAsync(entity);
        var readDto = _mapper.Map<MemberReadDto>(created);

        return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<ActionResult<MemberReadDto>> Get(int id)
    {
        return await base.Get(id);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<ActionResult<MemberReadDto>> Update(
        int id,
        [FromBody] MemberUpdateDto updateDto
    )
    {
        if (id != updateDto.Id)
        {
            return BadRequest();
        }

        var entity = _mapper.Map<Member>(updateDto);
        var updated = await _memberService.UpdateAsync(entity);
        var readDto = _mapper.Map<MemberReadDto>(updated);

        return Ok(readDto);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<IActionResult> Delete(int id)
    {
        return await base.Delete(id);
    }
}
