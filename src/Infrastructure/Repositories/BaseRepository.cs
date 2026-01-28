using Application.Repositories;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio genérico que define consultas a la base de datos para operaciones CRUD.
    /// </summary>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IIdentifiable
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Obtiene todas las entidades de todas las organizaciones.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las entidades de una organización.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(int organizationId)
        {
            var query = _dbSet
                .Where(e => EF.Property<int>(e, "OrganizationId") == organizationId);

            query = IncludeRelations(query);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene una entidad de una organización.
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var query = _dbSet.AsQueryable();
            query = IncludeRelations(query);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Crea una entidad perteneciente a una organización.
        /// </summary>
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Modifica una entidad perteneciente a una organización.
        /// </summary>
        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingEntity = await _dbSet.FindAsync(entity.Id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Categoria no encontrada.");
            }

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Elimina una entidad perteneciente a una organización.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Método virtual para incluir entidades compuestas mediante Eager Loading.
        /// </summary>
        /// <remarks>
        /// Los repositorios derivados deben sobrescribir este método para especificar qué
        /// propiedades de navegación deben ser cargadas junto con la entidad principal.
        /// Por defecto, no incluye ninguna relación (retorna la consulta sin modificaciones).
        /// </remarks>
        protected virtual IQueryable<T> IncludeRelations(IQueryable<T> query)
        {
            return query;
        }
    }
}
