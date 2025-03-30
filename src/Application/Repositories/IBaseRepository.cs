using Domain.Interfaces;

namespace Application.Repositories
{
    public interface IBaseRepository<T> where T : class, IIdentifiable
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(int organizationId);

        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
