using Mvc.Models;

namespace Mvc.Services.Interfaces
{
    public interface IMemberApiService
    {
        Task<List<MemberViewModel>> GetAllAsync(int organizationId);
        Task<MemberViewModel?> GetByIdAsync(int id);
        Task<MemberViewModel?> CreateAsync(MemberViewModel member);
        Task<MemberViewModel?> UpdateAsync(MemberViewModel member);
        Task<bool> DeleteAsync(int id);
    }
}
