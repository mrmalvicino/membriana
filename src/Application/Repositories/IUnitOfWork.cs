using Application.Services;

namespace Application.Repositories
{
    public interface IUnitOfWork
    {
        IIdentityService IdentityService { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IMemberRepository MemberRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
