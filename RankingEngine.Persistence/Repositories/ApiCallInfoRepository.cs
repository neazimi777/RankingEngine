using MongoDB.Driver;
using RankingEngine.Domain;
using RankingEngine.Domain.Repositories;

namespace RankingEngine.Persistence.Repositories
{
    public class ApiCallInfoRepository : GenericMongoRepository<ApiCallInfo>, IGenericMongoRepository<ApiCallInfo>
    {
        public ApiCallInfoRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public async Task<ApiCallInfo> GetActiveApiKey(string apiName, string id)
        {
            var filter = Builders<ApiCallInfo>.Filter.Empty;
            DateTime currentDate = DateTime.UtcNow;

            Builders<ApiCallInfo>.Filter.And(filter, Builders<ApiCallInfo>.Filter.Eq(e => e.Name, apiName));
            Builders<ApiCallInfo>.Filter.And(filter, Builders<ApiCallInfo>.Filter.Eq(e => e.Id, id));
            Builders<ApiCallInfo>.Filter.And(filter, Builders<ApiCallInfo>.Filter.Lt(e => e.ExpirationDate, currentDate));
            Builders<ApiCallInfo>.Filter.And(filter, Builders<ApiCallInfo>.Filter.Where(e => e.MaxCallPerMonth < e.CallCount));
            Builders<ApiCallInfo>.Filter.And(filter, Builders<ApiCallInfo>.Filter.Where(e => e.LatestCallTime.Add(e.DellayPerCall) < currentDate));

            var query = await GetQueryFilter(filter);
            var apiCallInfo = await query.FirstOrDefaultAsync();
            return apiCallInfo.Key;
        }
    }
}
