using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Infrastructures;
using ToDoWeb.DataAccess.Entities;

namespace ToDoWeb.DataAccess.Repositories.GenericAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? entityId, Expression<Func<T, object>>? expression = null)
        {
            var query = _dbSet.AsQueryable();
            if (entityId.HasValue)
            {
                query = query.Where(entity => entity.Id == entityId);
            }
           if (expression != null)
            {
                query = query.Include(expression);
            }
            var entitys = await query.ToListAsync();
            return entitys.Count == 0 ? Enumerable.Empty<T>() : entitys;
        }

        public async Task<T?> GetByIdAsync(int entityId)
        {
            return await _dbSet.FindAsync(entityId);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;

        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            //_dbContext.Entry(existingCourse).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            //var entity = await GetByIdAsync(entityId);
            //if (entity == null)
            //{
            //    throw new ArgumentException($"Entity with ID {entityId} does not exist.");
            //}
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;

        }
    }
}
