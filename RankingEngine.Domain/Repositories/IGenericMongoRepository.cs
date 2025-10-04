using MongoDB.Driver;

namespace RankingEngine.Domain.Repositories
{
    public interface IGenericMongoRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync(FilterDefinition<T> filter, int page = 2, int pagesize = 10);
        Task<IFindFluent<T, T>> GetQueryFilter(FilterDefinition<T> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(string id, T entity);
    }
}
