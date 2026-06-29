using Application.Services;

namespace Application.Repositories;

public interface IUnitOfWork
{
    IAppUserRepository AppUserRepository { get; }
    IIdentityService IdentityService { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IMemberRepository MemberRepository { get; }
    IMemberStatusEventRepository MemberStatusEventRepository { get; }
    IOrganizationRepository OrganizationRepository { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
