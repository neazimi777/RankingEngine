namespace RankingEngine.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public int ErrorCode { get; set; }
        public BaseException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

    }
}
