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
    public virtual async Task<ActionResult<IEnumerable<AmountResponse>>> CountMembersWithStatus(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month,
        [FromQuery] MemberStatus status
    )
    {
        int count = await _memberStatusService.CountMembersWithStatusAsync(year, month, status);
        var response = new List<AmountResponse> { new AmountResponse(count) };
        return Ok(response);
    }

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de alta por primera vez en un mes.
    /// </summary>
    [HttpGet("count-first-time-signups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public virtual async Task<ActionResult<IEnumerable<AmountResponse>>> CountFirstTimeSignups(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month
    )
    {
        int count = await _memberStatusService.CountFirstTimeSignupsAsync(year, month);
        var response = new List<AmountResponse> { new AmountResponse(count) };
        return Ok(response);
    }

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de baja por primera vez en un mes.
    /// </summary>
    [HttpGet("count-first-time-cancellations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public virtual async Task<ActionResult<IEnumerable<AmountResponse>>> CountFirstTimeCancellations(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month
    )
    {
        int count = await _memberStatusService.CountFirstTimeCancellationsAsync(year, month);
        var response = new List<AmountResponse> { new AmountResponse(count) };
        return Ok(response);
    }
}
