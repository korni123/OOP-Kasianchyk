namespace lab13v9;

public class ApiRequestFailedException : Exception
{
    public int StatusCode { get; }
    public string ResponseContent { get; } = "";

    public ApiRequestFailedException()
    {
    }

    public ApiRequestFailedException(string message) : base(message)
    {
    }

    public ApiRequestFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ApiRequestFailedException(string message, int statusCode, string responseContent)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }
}
