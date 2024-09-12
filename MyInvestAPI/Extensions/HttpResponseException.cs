namespace MyInvestAPI.Extensions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; set; }
        public object? Value { get; set; }
        public DateTime TimeStamp { get; set; }

        public HttpResponseException(int statusCode, string message)
        {
            StatusCode = statusCode;
            Value = new { message };
            TimeStamp = DateTime.UtcNow;
        }
    }
}
