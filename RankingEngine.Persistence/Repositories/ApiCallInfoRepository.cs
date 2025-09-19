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
    }
}
