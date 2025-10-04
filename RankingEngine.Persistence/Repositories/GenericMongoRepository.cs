using MongoDB.Bson;
using MongoDB.Driver;
using RankingEngine.Domain.Repositories;

namespace RankingEngine.Persistence.Repositories
{
    public class GenericMongoRepository<T> : IGenericMongoRepository<T> where T : class
    {
        private IMongoCollection<T> _collection;

        public GenericMongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(t => true).ToListAsync();
        }
        public async Task<List<T>> GetAllAsync(FilterDefinition<T> filter, int page = 2, int pagesize = 10)
        {
            var query = _collection.Find(filter ?? Builders<T>.Filter.Empty)
                              .Skip((page - 1) * pagesize)
                              .Limit(pagesize);
            return await query.ToListAsync();
        }
        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        public async Task<IFindFluent<T, T>> GetQueryFilter(FilterDefinition<T> filter)
        {
            return _collection.Find(filter ?? Builders<T>.Filter.Empty);
        }
        public async Task<bool> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
            return result.DeletedCount > 0;
        }
        public async Task<bool> UpdateAsync(string id, T entity)
        {
            var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)), entity);
            return result.ModifiedCount > 0 || result.UpsertedId != null;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await _collection.InsertManyAsync(entities);
            return true;
        }
    }
}
