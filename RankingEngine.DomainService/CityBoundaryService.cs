using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RankingEngine.Domain;
using RankingEngine.Domain.Repositories;
using RankingEngine.DomainService.Abstractions;
using RankingEngine.Dto.ApiRequestResponse;

namespace RankingEngine.DomainService
{
    public class CityBoundaryService : ICityBoundaryService
    {
        private readonly IApiService _api;
        private readonly ILogger<CityBoundaryService> _logger;
        private readonly ICityBoundaryRepository _cityBoundaryRepository;
        private readonly IConfiguration _configuration;

        public CityBoundaryService(IApiService api, ILogger<CityBoundaryService> logger, IConfiguration configuration,
            ICityBoundaryRepository cityBoundaryRepository)
        {
            _api = api;
            _logger = logger;
            _cityBoundaryRepository = cityBoundaryRepository;
            _configuration = configuration;
        }
        public async Task<ApiResponseDto<CityBoundary>> GenerateCityBoundary(string city, string country, CancellationToken ct = default)
        {
            var query = new Dictionary<string, string?>
            {
                ["city"] = city,
                ["country"] = country,
                ["format"] = "jsonv2",
                ["polygon_geojson"] = "1"
            };

            var req = new ApiRequestDto(
                method: HttpMethod.Get,
                url: _configuration.GetValue<string>("CityBoundaryUrl"),
                queryParams: query
            );

            var policy = new ResiliencePolicyOptions
            {
                RetryEnable = true,
                RetryCount = 3,
                RetryPowAttempt = 2,
                CircuitBreakerEnable = true,
                CircuitBreakerFailuresAllowed = 3,
                CircuitBreakerBreakDurationSeconds = 15
            };

            return await _api.SendAsync<CityBoundary>(req, policy, ct);
        }

        public async Task<CityBoundary> GetCityBoundary(string city, string country, CancellationToken ct = default)
        {
           return await _cityBoundaryRepository.GetCityBoundary(city, country, ct);
        }

        public async Task<bool> SaveCityBoundary(string city, string country, CancellationToken ct = default)
        {
            var cityboundary = await GenerateCityBoundary(city, country, ct);
            return await _cityBoundaryRepository.AddAsync(cityboundary.Data);
        }
    }
}
