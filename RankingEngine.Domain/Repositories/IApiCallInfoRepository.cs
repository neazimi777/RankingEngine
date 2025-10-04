namespace RankingEngine.Domain.Repositories
{
    public interface IApiCallInfoRepository:IGenericMongoRepository<ApiCallInfo>
    {
        Task<ApiCallInfo> GetActiveApiKey(string apiName, string id);
    }
}
