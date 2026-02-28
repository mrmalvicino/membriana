using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Filters;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[JwtAuthorizationFilter]
public class EmployeeController : Controller
{
    private readonly IEmployeeApiService _employeeApi;
    private readonly IUserApiService _userApi;

    public EmployeeController(
        IEmployeeApiService employeeService,
        IUserApiService userService
    )
    {
        _employeeApi = employeeService;
        _userApi = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizationId = await _userApi.GetOrganizationIdAsync();
        var employees = await _employeeApi.GetAllAsync(organizationId);
        return View(employees);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeApi.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userOrgId = await _userApi.GetOrganizationIdAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel employee)
    {
        if (ModelState.IsValid)
        {
            employee.OrganizationId = await _userApi.GetOrganizationIdAsync();
            await _employeeApi.CreateAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        var userOrgId = await _userApi.GetOrganizationIdAsync();

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeApi.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeViewModel employee)
    {
        employee.OrganizationId = await _userApi.GetOrganizationIdAsync();

        if (ModelState.IsValid)
        {
            await _employeeApi.UpdateAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeApi.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        if (employee.OrganizationId != await _userApi.GetOrganizationIdAsync())
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeApi.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
