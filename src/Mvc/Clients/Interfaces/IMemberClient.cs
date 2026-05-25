using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IMemberClient
{
    Task<List<MemberViewModel>> GetAllAsync(int organizationId);
    Task<MemberViewModel?> GetByIdAsync(int id);
    Task<MemberViewModel?> CreateAsync(MemberViewModel member);
    Task<MemberViewModel?> UpdateAsync(MemberViewModel member);
    Task<bool> DeleteAsync(int id);
}
