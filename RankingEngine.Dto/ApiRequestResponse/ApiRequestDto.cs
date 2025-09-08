namespace RankingEngine.Dto.ApiRequestResponse
{
    public class ApiRequestDto
    {
        public HttpMethod Method { get; set; } = HttpMethod.Get;
        public string Url { get; set; } = string.Empty;
        public object? Body { get; set; }
        public IDictionary<string, string>? Headers { get; set; }
        public IDictionary<string, string?>? QueryParams { get; set; }
        public ApiRequestDto() { }
        public ApiRequestDto(HttpMethod method, string url, object? body = null,
                          IDictionary<string, string>? headers = null,
                          IDictionary<string, string?>? queryParams = null)
        {
            Method = method;
            Url = url;
            Body = body;
            Headers = headers;
            QueryParams = queryParams;
        }
    }

}
