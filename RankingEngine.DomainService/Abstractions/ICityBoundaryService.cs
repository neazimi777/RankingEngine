using RankingEngine.Domain;
using RankingEngine.Dto.ApiRequestResponse;

namespace RankingEngine.DomainService.Abstractions
{
    public interface ICityBoundaryService
    {
        Task<ApiResponseDto<CityBoundary>> GenerateCityBoundary(string city, string country, CancellationToken ct = default);
        Task<CityBoundary> GetCityBoundary(string city, string country, CancellationToken ct = default);
        Task<bool> SaveCityBoundary(string city, string country, CancellationToken ct = default);
    }
}
