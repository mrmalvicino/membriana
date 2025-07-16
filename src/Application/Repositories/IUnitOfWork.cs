namespace Application.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        IOrganizationRepository OrganizationRepository { get; }
    }

}
