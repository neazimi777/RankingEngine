using RankingEngine.DomainService.Abstractions;

namespace RankingEngine.DomainService
{
    public class GoogleApiService
    {
        private readonly IJobSchedulerService _jobSchedulerService;
        private readonly IApiCallInfoService _apiCallInfoService;
        private readonly IApiService _apiService;
        private readonly ICityGeoPointService _cityGeoPointService;
        public GoogleApiService(IJobSchedulerService jobSchedulerService,
            IApiService apiService
            ,IApiCallInfoService apiCallInfoService,
             ICityGeoPointService cityGeoPointService)
        {
            _apiCallInfoService = apiCallInfoService;
            _jobSchedulerService = jobSchedulerService;
            _apiService = apiService;
            _cityGeoPointService = cityGeoPointService;
        }


    }
}
