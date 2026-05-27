using Application.Services;
using Api.Helpers;
using Contracts.Dtos.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<LoggedUserContextDto>> GetLoggedUserContext()
    {
        try
        {
            var currentUserContextDto = await _userService.GetLoggedUserContextAsync();
            return Ok(currentUserContextDto);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ErrorResponseFactory.Create(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ErrorResponseFactory.Create(ex.Message));
        }
    }

    [HttpGet("me/organization-id")]
    public async Task<ActionResult<int>> GetOrganizationId()
    {
        try
        {
            var organizationId = await _userService.GetOrganizationIdAsync();
            return Ok(organizationId);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ErrorResponseFactory.Create(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ErrorResponseFactory.Create(ex.Message));
        }
    }
}
