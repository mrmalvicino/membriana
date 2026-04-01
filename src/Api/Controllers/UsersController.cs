using Application.Services;
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
