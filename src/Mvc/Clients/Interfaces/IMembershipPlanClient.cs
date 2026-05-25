using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IMembershipPlanClient
{
    Task<List<MembershipPlanViewModel>> GetAllAsync(int organizationId);
    Task<MembershipPlanViewModel?> GetByIdAsync(int id);
    Task<MembershipPlanViewModel?> CreateAsync(MembershipPlanViewModel membershipPlan);
    Task<MembershipPlanViewModel?> UpdateAsync(MembershipPlanViewModel membershipPlan);
    Task DeleteAsync(int id);
}
