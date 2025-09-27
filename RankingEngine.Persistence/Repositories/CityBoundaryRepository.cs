using MongoDB.Driver;
using RankingEngine.Domain;
using RankingEngine.Domain.Repositories;

namespace RankingEngine.Persistence.Repositories
{
    public class CityBoundaryRepository : GenericMongoRepository<CityBoundary>, ICityBoundaryRepository
    {
        public CityBoundaryRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public async Task<CityBoundary> GetCityBoundary(string city, string country, CancellationToken ct = default)
        {
            var filter = Builders<CityBoundary>.Filter.Empty;

            Builders<CityBoundary>.Filter.And(filter, Builders<CityBoundary>.Filter.Eq(e => e.CityName, city));
            Builders<CityBoundary>.Filter.And(filter, Builders<CityBoundary>.Filter.Eq(e => e.CountryName, city));
            
            var query = await GetQueryFilter(filter);
            return await query.FirstOrDefaultAsync();
        }
    }
}
