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
        Task AddAsync(T model);
        Task UpdateAsync(T model);
        Task UpdateByIdAsync(Guid id);
        Task BatchUpdateAsync(IEnumerable<T> entities);
        Task RemoveAsync(T model);
        Task RemoveByIdAsync(Guid id);
        Task BatchRemoveAsync(IEnumerable<T> entities);
    }
}