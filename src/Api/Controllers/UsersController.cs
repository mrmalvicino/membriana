using Application.Services;
using Api.Helpers;
using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly IUserService _userService;

    public UsersController(IUserService userService, IUserManagementService userManagementService)
    {
        _userService = userService;
        _userManagementService = userManagementService;
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

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
    {
        var organizationId = await _userService.GetOrganizationIdAsync();
        return Ok(await _userManagementService.GetAllAsync(organizationId));
    }

    [HttpGet("eligible-members")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<UserCandidateDto>>> GetEligibleMembers()
    {
        var organizationId = await _userService.GetOrganizationIdAsync();
        return Ok(await _userManagementService.GetEligibleMembersAsync(organizationId));
    }

    [HttpGet("eligible-employees")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<UserCandidateDto>>> GetEligibleEmployees()
    {
        var organizationId = await _userService.GetOrganizationIdAsync();
        return Ok(await _userManagementService.GetEligibleEmployeesAsync(organizationId));
    }

    [HttpPost("members/{memberId}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<RegisterResponseDto>> CreateForMember(int memberId)
    {
        var organizationId = await _userService.GetOrganizationIdAsync();

        try
        {
            return Ok(await _userManagementService.CreateUserForMemberAsync(memberId, organizationId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ErrorResponseFactory.Create(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ErrorResponseFactory.Create(ex.Message));
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ErrorResponseFactory.Create(ex.Message));
        }
    }

    [HttpPost("employees/{employeeId}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<RegisterResponseDto>> CreateForEmployee(int employeeId)
    {
        var organizationId = await _userService.GetOrganizationIdAsync();

        try
        {
            return Ok(await _userManagementService.CreateUserForEmployeeAsync(employeeId, organizationId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ErrorResponseFactory.Create(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ErrorResponseFactory.Create(ex.Message));
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ErrorResponseFactory.Create(ex.Message));
        }
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserReadDto>> GetById(string id)
    {
        var organizationId = await _userService.GetOrganizationIdAsync();
        var user = await _userManagementService.GetByIdAsync(id, organizationId);

        if (user == null)
        {
            return NotFound(ErrorResponseFactory.Create("El usuario no existe."));
        }

        return Ok(user);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var organizationId = await _userService.GetOrganizationIdAsync();
        var loggedUser = await _userService.GetLoggedUserAsync();

        try
        {
            await _userManagementService.DeleteAsync(id, organizationId, loggedUser.Id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ErrorResponseFactory.Create(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ErrorResponseFactory.Create(ex.Message));
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ErrorResponseFactory.Create(ex.Message));
        }
    }
}
