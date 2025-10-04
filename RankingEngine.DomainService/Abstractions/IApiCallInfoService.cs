using RankingEngine.Domain;
using RankingEngine.Domain.Repositories;

namespace RankingEngine.DomainService.Abstractions
{
    public interface IApiCallInfoService
    {
        Task<ApiCallInfo> GetActiveApiKey(string apiName, string id);
    }
}
