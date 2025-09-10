using RankingEngine.Dto.ApiRequestResponse;

namespace RankingEngine.DomainService.Abstractions
{
    public interface IApiService
    {
        Task<ApiResponseDto<T>> SendAsync<T>(ApiRequestDto apiRequestDto, ResiliencePolicyOptions resiliencePolicy, CancellationToken cancellationToken = default);
    }
}
