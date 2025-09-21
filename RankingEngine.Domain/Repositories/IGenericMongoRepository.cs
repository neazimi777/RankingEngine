using MongoDB.Driver;

namespace RankingEngine.Domain.Repositories
{
    public interface IGenericMongoRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync(FilterDefinition<T> filter, int page = 2, int pagesize = 10);
        Task<IFindFluent<T, T>> GetQueryFilter(FilterDefinition<T> filter);
        Task AddAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(string id, T entity);
    }
}
