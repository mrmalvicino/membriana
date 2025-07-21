using Mvc.Models;

namespace Mvc.Services.Api.Interfaces
{
    public interface IMembershipPlanApiService
    {
        Task<List<MembershipPlanViewModel>> GetAllAsync(int organizationId);
        Task<MembershipPlanViewModel?> GetByIdAsync(int id);
        Task<MembershipPlanViewModel?> CreateAsync(MembershipPlanViewModel membershipPlan);
        Task<MembershipPlanViewModel?> UpdateAsync(MembershipPlanViewModel membershipPlan);
        Task DeleteAsync(int id);
    }
}
