using RankingEngine.Dto.ApiRequestResponse;

namespace RankingEngine.DomainService.Abstractions
{
    public interface IApiService
    {
        Task<ApiResponseDto<T>> SendAsync<T>(ApiRequestDto apiRequestDto, CancellationToken cancellationToken = default);
    }
}
