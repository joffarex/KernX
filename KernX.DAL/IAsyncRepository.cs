using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KernX.DAL
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task BatchUpdateAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveByIdAsync(Guid id);
        Task BatchRemoveAsync(IEnumerable<T> entities);
    }
}