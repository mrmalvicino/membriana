using Application.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        private readonly IOrganizationRepository _organizationRepository;

        public UnitOfWork(
            AppDbContext dbContext,
            IOrganizationRepository organizationRepository
        )
        {
            _dbContext = dbContext;
            _organizationRepository = organizationRepository;
        }

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

        public IOrganizationRepository OrganizationRepository => _organizationRepository;
    }
}
