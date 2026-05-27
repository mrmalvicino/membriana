using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Authentication;
using Mvc.Exceptions;
using Mvc.Clients.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class EmployeeController : Controller
{
    private readonly IEmployeeClient _employeeClient;
    private readonly IUserClient _userClient;

    public EmployeeController(
        IEmployeeClient employeeClient,
        IUserClient userClient
    )
    {
        _employeeClient = employeeClient;
        _userClient = userClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizationId = await _userClient.GetOrganizationIdAsync();
        var employees = await _employeeClient.GetAllAsync(organizationId);
        return View(employees);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeClient.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userOrgId = await _userClient.GetOrganizationIdAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                employee.OrganizationId = await _userClient.GetOrganizationIdAsync();
                await _employeeClient.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeClient.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeViewModel employee)
    {
        employee.OrganizationId = await _userClient.GetOrganizationIdAsync();

        if (ModelState.IsValid)
        {
            try
            {
                await _employeeClient.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeClient.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userClient.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _employeeClient.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (BusinessRuleException ex)
        {
            TempData["BusinessError"] = ex.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }
        catch (ApplicationException ex)
        {
            TempData["BusinessError"] = ex.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
