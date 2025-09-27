using Microsoft.AspNetCore.Http;
using RankingEngine.Domain.Exceptions;
using RankingEngine.Domain.Repositories;
using RankingEngine.DomainService.Abstractions;

namespace RankingEngine.DomainService
{
    public class CityGeoPointService: ICityGeoPointService
    {
        private readonly ICityGeoPointRepository _cityGeoPointRepository;
        private readonly ICityBoundaryRepository _cityBoundaryRepository;
        private readonly IJobSchedulerService _jobSchedulerService;
        private readonly IApiService _apiService;
        public CityGeoPointService(ICityGeoPointRepository cityGeoPointRepository,
            ICityBoundaryRepository cityBoundaryRepository,
            IJobSchedulerService jobSchedulerService,
            IApiService apiService)
        {
            _cityGeoPointRepository = cityGeoPointRepository;
            _cityBoundaryRepository = cityBoundaryRepository;
            _jobSchedulerService = jobSchedulerService;
            _apiService = apiService;
        }

        public async Task<>GenerateCityGeoPoints(string city, string country,CancellationToken ct =default)
        {
           var  result = await _cityBoundaryRepository.GetCityBoundary(city, country,ct);
            if (result is null)
                throw new BaseException($"City :{city} and Country :{country} boundary not found.", (int)StatusCodes.Status400BadRequest);

            


        }
    }
}
