using Application.Services;

namespace Application.Repositories
{
    public interface IUnitOfWork
    {
        IIdentityService IdentityService { get; }
        IOrganizationRepository OrganizationRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
