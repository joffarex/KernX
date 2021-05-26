using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace KernX.DAL.MongoDB
{
    public class MongoAsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _entities;

        public MongoAsyncRepository(MongoDBSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _entities = database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IAsyncCursor<T> documents = await _entities.FindAsync(e => true);

            return await documents.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> documents = await _entities.FindAsync(filter);

            return await documents.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            IAsyncCursor<T> documents = await _entities.FindAsync(e => e.Id == id);

            return await documents.FirstOrDefaultAsync();
        }

        public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> documents = await _entities.FindAsync(filter);

            return await documents.FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
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
            await _entities.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            await _entities.DeleteOneAsync(e => e.Id == id);
        }

        public async Task BatchRemoveAsync(IEnumerable<T> entities)
        {
            await _entities.DeleteManyAsync(e => entities.Select(entity => entity.Id).Contains(e.Id));
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            await _entities.InsertManyAsync(entities);
        }
    }
}