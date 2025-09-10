namespace RankingEngine.Dto.ApiRequestResponse
{
    public class ResiliencePolicyOptions
    {
        #region Retry Policy
        public bool RetryEnable { get; set; }
        public int RetryCount { get; set; }
        public int RetryPowAttempt { get; set; }
        #endregion

        #region CircuitBreaker Policy
        public bool CircuitBreakerEnable { get; set; }
        public int CircuitBreakerFailuresAllowed { get; set; }
        public int CircuitBreakerBreakDurationSeconds { get; set; }
        #endregion
    }
}
