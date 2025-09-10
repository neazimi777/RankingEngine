using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly.Extensions.Http;
using Polly;
using RankingEngine.DomainService.Abstractions;
using RankingEngine.Dto.ApiRequestResponse;
using System.Text;
using System;


namespace RankingEngine.DomainService
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient http, ILogger<ApiService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<ApiResponseDto<T>> SendAsync<T>(ApiRequestDto apiRequestDto, ResiliencePolicyOptions resiliencePolicy, CancellationToken cancellationToken = default)
        {
            var finalUrl = BuildUri(apiRequestDto.Url, apiRequestDto.QueryParams);
            using var request = new HttpRequestMessage(apiRequestDto.Method, finalUrl);

            if (apiRequestDto.Body != null)
                request.Content = ToJsonContent(apiRequestDto.Body);

            AddHeaders(request, apiRequestDto.Headers);

            _logger.LogDebug("Sending {Method} {Url}", apiRequestDto.Method, finalUrl);

            var retryPolicy = GetRetryPolicy(resiliencePolicy.RetryEnable, resiliencePolicy.RetryCount, resiliencePolicy.RetryPowAttempt) ?? Policy.NoOpAsync<HttpResponseMessage>();
            var cbPolicy = GetCircuitBreakerPolicy(resiliencePolicy.CircuitBreakerEnable, resiliencePolicy.CircuitBreakerFailuresAllowed, resiliencePolicy.CircuitBreakerBreakDurationSeconds)
                           ?? Policy.NoOpAsync<HttpResponseMessage>();

            var policyWrap = Policy.WrapAsync(retryPolicy, cbPolicy);


            var response = await policyWrap.ExecuteAsync(() => _http.SendAsync(request, cancellationToken));
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Request to {Url} failed {StatusCode}", finalUrl, (int)response.StatusCode);
                return new ApiResponseDto<T>((int)response.StatusCode, response.Content.ToString());
            }

            if (typeof(T) == typeof(string))
                return new ApiResponseDto<T>((T)(object)raw, (int)response.StatusCode, response.Content.ToString());

            var deserialized = Deserialize<T>(raw)!;
            return new ApiResponseDto<T>(deserialized, (int)response.StatusCode, response.Content.ToString());
        }


        #region private method
        private StringContent ToJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        private T? Deserialize<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw);
        }
        private static string BuildUri(string baseUrl, IDictionary<string, string?>? queryParams)
        {
            if (queryParams == null || !queryParams.Any()) return baseUrl;

            var baseUri = baseUrl;
            var q = string.Join("&", queryParams
                .Where(kv => kv.Value != null)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

            return baseUri + (baseUrl.Contains("?") ? "&" : "?") + q;
        }

        private void AddHeaders(HttpRequestMessage request, IDictionary<string, string>? headers)
        {
            if (headers == null) return;
            foreach (var (k, v) in headers)
            {
                if (!request.Headers.TryAddWithoutValidation(k, v))
                    request.Content?.Headers.TryAddWithoutValidation(k, v);
            }
        }
        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(bool enable, int retryCount, int powAttempt)
        {
            if (!enable) return null;
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(powAttempt, attempt)),
                    (outcome, timespan, retryAttempt, context) =>
                    {
                        _logger.LogDebug($"Retry {retryAttempt} after {timespan.TotalSeconds}s");
                    });
        }

        private IAsyncPolicy<HttpResponseMessage>? GetCircuitBreakerPolicy(bool enable, int failuresAllowed, int breakDurationSeconds)
        {
            if (!enable) return null;
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    failuresAllowed,
                    TimeSpan.FromSeconds(breakDurationSeconds),
                    onBreak: (outcome, timespan) => _logger.LogDebug($"Circuit open for {timespan.TotalSeconds}s"),
                    onReset: () => _logger.LogDebug("Circuit closed"),
                    onHalfOpen: () => _logger.LogDebug("Circuit half-open, testing...")
                );
        }

        #endregion

    }
}






