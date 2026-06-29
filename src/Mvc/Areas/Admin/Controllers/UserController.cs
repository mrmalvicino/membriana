using Microsoft.AspNetCore.Mvc;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;

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
}
