using RankingEngine.Domain.Repositories;
using RankingEngine.DomainService.Abstractions;
using RankingEngine.Dto.ApiRequestResponse;

namespace RankingEngine.DomainService
{
    public class ApiCallInfoService : IApiCallInfoService
    {
        private readonly IApiCallInfoRepository _apiCallInfoRepository;
        public ApiCallInfoService(IApiCallInfoRepository apiCallInfoRepository)
        {
            _apiCallInfoRepository = apiCallInfoRepository;
        }


        public Task<string> GetActiveApiKey(string apiName , )
        {

        }
        public Task<bool> AddApiCallInfo(ApiCallInfoRequestDto apiCallInfoDto)
        {

        }
        public Task<bool> DeleteApiCallInfo()
        {

        }
        public Task<bool> UpdateApiCallInfo()
        {

        }
    }
}
