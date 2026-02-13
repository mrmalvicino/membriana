using Domain.Interfaces;

namespace Application.Repositories
{
    public interface IBaseRepository<T> where T : class, IIdentifiable
    {
        /// <summary>
        /// Obtiene todas las entidades de todas las organizaciones.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene todas las entidades de una organización.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(int organizationId);

        /// <summary>
        /// Obtiene una entidad de una organización.
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Crea una entidad perteneciente a una organización.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Modifica una entidad perteneciente a una organización.
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad perteneciente a una organización.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
