namespace RankingEngine.DomainService.Abstractions
{
    public interface ICityBoundaryService
    {
        public Task<bool> CreateCityBoundary();
    }
}
