using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using ToDoWeb.DataAccess.Entities;
using ToDoWeb.DataAccess.Repositories.GenericAccess;

namespace ToDoWeb.DataAccess.Repositories.CacheAccess
{
    public class CacheRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IGenericRepository<T> _decoratee;
        public CacheRepository(IGenericRepository<T> decoratee, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _decoratee = decoratee;
        }

        private string GetCacheKey(int entityId)
        {
            return $"{typeof(T).FullName}_{entityId}";
        }

        public async Task<int> AddAsync(T entity)
        {
            var cacheKey = GetCacheKey(entity.Id);
            _memoryCache.Set(cacheKey, entity);
            _memoryCache.Remove($"{typeof(T).FullName}_All");
            return await _decoratee.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? entityId, Expression<Func<T, object>>? expression = null)
        {
            var cacheKey = entityId.HasValue ? GetCacheKey(entityId.GetValueOrDefault()) : $"{typeof(T).FullName}_All";
            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                return await _decoratee.GetAllAsync(entityId, expression);
            }) ?? Enumerable.Empty<T>();
        }

        public async Task<T?> GetByIdAsync(int entityId)
        {
            var cacheKey = GetCacheKey(entityId);
            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                return await _decoratee.GetByIdAsync(entityId);
            });
        }

        public Task<int> UpdateAsync(T entity)
        {
           var cacheKey = GetCacheKey(entity.Id);
            _memoryCache.Set(cacheKey, entity);
            _memoryCache.Remove($"{typeof(T).FullName}_All");
            return _decoratee.UpdateAsync(entity);
        }

        public Task<int> DeleteAsync(T entity)
        {
            var cacheKey = GetCacheKey(entity.Id);
            _memoryCache.Remove(cacheKey);
            _memoryCache.Remove($"{typeof(T).FullName}_All");
            return _decoratee.DeleteAsync(entity);
        }
    }
}
