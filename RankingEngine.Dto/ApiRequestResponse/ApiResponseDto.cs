namespace RankingEngine.Dto.ApiRequestResponse
{
    public class ApiResponseDto <T>
    {
        public T Data { get; }
        public int StatusCode { get; }
        public string? Message { get; }
        public ApiResponseDto(T data, int statusCode = 200, string? message = null)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }
        public ApiResponseDto( int statusCode = 200, string? message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
