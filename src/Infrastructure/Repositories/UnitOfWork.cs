using Application.Repositories;
using Application.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        private readonly IIdentityService _identityService;
        private readonly IOrganizationRepository _organizationRepository;

        public UnitOfWork(
            AppDbContext dbContext,
            IIdentityService identityService,
            IOrganizationRepository organizationRepository
        )
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _organizationRepository = organizationRepository;
        }

        public IIdentityService IdentityService => _identityService;
        public IOrganizationRepository OrganizationRepository => _organizationRepository;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
