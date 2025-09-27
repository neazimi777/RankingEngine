using MongoDB.Driver;
using RankingEngine.Domain;
using RankingEngine.Domain.Repositories;

namespace RankingEngine.Persistence.Repositories
{
    public class CityGeoPointRepository : GenericMongoRepository<CityGeoPoint>, ICityGeoPointRepository
    {
        public CityGeoPointRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }
    }
}
