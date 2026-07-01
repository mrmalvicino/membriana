using Microsoft.AspNetCore.Mvc;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;
using Mvc.Exceptions;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class UserController : AdminControllerBase
{
    private readonly IUserClient _userClient;

    public UserController(IUserClient userClient)
    {
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userClient.GetAllAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> CreateUserForMember()
    {
        var candidates = await _userClient.GetEligibleMembersAsync();
        return View(nameof(CreateUserForMember), candidates);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUserForMember(int id)
    {
        try
        {
            TempData["UserInviteIsSuccess"] = true;
            TempData["UserInviteMessage"] = await _userClient.CreateUserForMemberAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (TrySetInviteError(ex))
        {
            return RedirectToAction(nameof(CreateUserForMember));
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateUserForEmployee()
    {
        var candidates = await _userClient.GetEligibleEmployeesAsync();
        return View(nameof(CreateUserForEmployee), candidates);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUserForEmployee(int id)
    {
        try
        {
            TempData["UserInviteIsSuccess"] = true;
            TempData["UserInviteMessage"] = await _userClient.CreateUserForEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (TrySetInviteError(ex))
        {
            return RedirectToAction(nameof(CreateUserForEmployee));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userClient.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            await _userClient.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex) when (TrySetDeleteError(ex))
        {
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    private bool TrySetInviteError(Exception exception)
    {
        if (exception is not (KeyNotFoundException or BusinessRuleException or ApplicationException))
        {
            return false;
        }

        TempData["UserInviteIsSuccess"] = false;
        TempData["UserInviteMessage"] = exception.Message;
        return true;
    }
}
