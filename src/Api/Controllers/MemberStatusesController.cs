using Api.Filters;
using Application.Services;
using Contracts.Dtos.MemberStatus;
using Contracts.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Policy = "Employee")]
[ApiController]
[Route("api/[controller]")]
public class MemberStatusesController : ControllerBase
{
    private readonly IMemberStatusService _memberStatusService;

    /// <summary>
    /// Constructor principal.
    /// </summary>
    public MemberStatusesController(IMemberStatusService memberStatusService)
    {
        _memberStatusService = memberStatusService;
    }

    /// <summary>
    /// Obtiene la cantidad de miembros con un estado en particular.
    /// </summary>
    [HttpGet("count-members-with-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public async Task<ActionResult<AmountResponse>> CountMembersWithStatus(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month,
        [FromQuery] MemberStatus status
    )
    {
        int count = await _memberStatusService.CountMembersWithStatusAsync(organizationId, year, month, status);
        return Ok(new AmountResponse(count));
    }

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de alta por primera vez en un mes.
    /// </summary>
    [HttpGet("count-first-time-signups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public async Task<ActionResult<AmountResponse>> CountFirstTimeSignups(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month
    )
    {
        int count = await _memberStatusService.CountFirstTimeSignupsAsync(organizationId, year, month);
        return Ok(new AmountResponse(count));
    }

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de baja por primera vez en un mes.
    /// </summary>
    [HttpGet("count-first-time-cancellations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public async Task<ActionResult<AmountResponse>> CountFirstTimeCancellations(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month
    )
    {
        int count = await _memberStatusService.CountFirstTimeCancellationsAsync(organizationId, year, month);
        return Ok(new AmountResponse(count));
    }
}
