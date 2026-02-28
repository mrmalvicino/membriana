using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Services.Api.Interfaces;

public interface IEmployeeApiService
{
    Task<List<EmployeeViewModel>> GetAllAsync(int organizationId);
    Task<EmployeeViewModel?> GetByIdAsync(int id);
    Task<EmployeeViewModel?> CreateAsync(EmployeeViewModel employee);
    Task<EmployeeViewModel?> UpdateAsync(EmployeeViewModel employee);
    Task<bool> DeleteAsync(int id);
}
