using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoWeb.DataAccess.Entities;

namespace ToDoWeb.DataAccess.Repositories.GenericAccess
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<int> AddAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(int? entityId, Expression<Func<T, object>>? expression = null);
        Task<T?> GetByIdAsync(int entityId);
        Task<int> UpdateAsync(T entity);
    }
}
