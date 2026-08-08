using Api.Filters;
using Api.Helpers;
using Contracts.Dtos.Member;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class MembersController : BaseController<
    Member,
    IMemberRepository,
    MemberReadDto,
    MemberCreateDto,
    MemberUpdateDto
>
{
    private readonly IMemberRepository _repository;
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
        _repository = repository;
        _memberService = memberService;
    }

    [HttpGet]
    [Authorize(Policy = "Employee")]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public override async Task<ActionResult<IEnumerable<MemberReadDto>>> GetAll(
        [FromQuery] int organizationId
    )
    {
        return await base.GetAll(organizationId);
    }

    [Authorize(Policy = "Employee")]
    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<ActionResult<MemberReadDto>> Get(int id)
    {
        return await base.Get(id);
    }

    [HttpGet("me")]
    [Authorize(Policy = "Member")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberReadDto>> GetForLoggedUser()
    {
        var loggedUserContext = await _userService.GetLoggedUserContextAsync();

        if (!loggedUserContext.MemberId.HasValue)
        {
            return Forbid();
        }

        var member = await _repository.GetByIdAsync(loggedUserContext.MemberId.Value);

        if (member == null)
        {
            return NotFound(ErrorResponseFactory.Create("El recurso no existe."));
        }

        var readDto = _mapper.Map<MemberReadDto>(member);
        return Ok(readDto);
    }

    [Authorize(Policy = "Employee")]
    public override async Task<ActionResult<MemberReadDto>> Create([FromBody] MemberCreateDto createDto)
    {
        int userOrgId = await _userService.GetOrganizationIdAsync();

        if (createDto.OrganizationId != userOrgId)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                ErrorResponseFactory.Create("No tenés permisos para crear recursos en otra organización.")
            );
        }

        var entity = _mapper.Map<Member>(createDto);

        try
        {
            var created = await _memberService.AddAsync(entity);
            var readDto = _mapper.Map<MemberReadDto>(created);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }
        catch (DbUpdateException ex) when (DbUpdateExceptionHelper.TryCreateConflictMessage(ex, out var message))
        {
            return Conflict(ErrorResponseFactory.Create(message));
        }
    }

    [Authorize(Policy = "Employee")]
    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<ActionResult<MemberReadDto>> Update(
        int id,
        [FromBody] MemberUpdateDto updateDto
    )
    {
        if (id != updateDto.Id)
        {
            return BadRequest(ErrorResponseFactory.Create("El ID de la ruta no coincide con el ID del recurso."));
        }

        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return NotFound(ErrorResponseFactory.Create("El recurso no existe."));
        }

        _mapper.Map(updateDto, entity);

        try
        {
            var updated = await _memberService.UpdateAsync(entity);
            var readDto = _mapper.Map<MemberReadDto>(updated);

            return Ok(readDto);
        }
        catch (DbUpdateException ex) when (DbUpdateExceptionHelper.TryCreateConflictMessage(ex, out var message))
        {
            return Conflict(ErrorResponseFactory.Create(message));
        }
    }

    [Authorize(Policy = "Employee")]
    [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
    public override async Task<IActionResult> Delete(int id)
    {
        return await base.Delete(id);
    }
}
