using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace KernX.DAL.MongoDB
{
    public class MongoAsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoAsyncRepository(MongoDBSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IAsyncCursor<T> documents = await _collection.FindAsync(e => true);

            return await documents.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> documents = await _collection.FindAsync(filter);

            return await documents.ToListAsync();
        }

        public async Task<PagedResult<T>> PaginateAsync
            (Expression<Func<T, bool>> filter, PagedQuery query) =>
            await _collection.AsQueryable().Where(filter).PaginateAsync(query);

        public async Task<T> GetByIdAsync(Guid id)
        {
            IAsyncCursor<T> documents = await _collection.FindAsync(e => e.Id == id);

            return await documents.FirstOrDefaultAsync();
        }

        public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> documents = await _collection.FindAsync(filter);

            return await documents.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync
            (Expression<Func<T, bool>> filter) => await _collection.FindSync(filter).AnyAsync();

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task BatchUpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            await _collection.DeleteOneAsync(e => e.Id == id);
        }

        public async Task BatchRemoveAsync(IEnumerable<T> entities)
        {
            await _collection.DeleteManyAsync(e => entities.Select(entity => entity.Id).Contains(e.Id));
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }
    }
}