using Microsoft.AspNetCore.Http;
using RankingEngine.Domain;
using RankingEngine.Domain.Exceptions;
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


        public async Task<ApiCallInfo> GetActiveApiKey(string apiName ,string id)
        {
            var apiInfo =  await _apiCallInfoRepository.GetActiveApiKey(apiName, id);
            if (apiInfo is null)
                throw new BaseException($"ApiInfo with name : {apiName} not found", StatusCodes.Status400BadRequest);

            return apiInfo;
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
