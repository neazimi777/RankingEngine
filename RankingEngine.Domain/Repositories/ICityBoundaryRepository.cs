namespace RankingEngine.Domain.Repositories
{
    public interface ICityBoundaryRepository : IGenericMongoRepository<CityBoundary>
    {
        Task<CityBoundary> GetCityBoundary(string city, string country, CancellationToken ct = default);
    }
}
