using System.Text.Json;

namespace lab13v9;

public class ApiService
{
    public string MakeRequest(string url)
    {
        int statusCode = GetStatusCode(url);
        if (statusCode >= 400)
        {
            throw new ApiRequestFailedException(
                $"Request to {url} failed with status {statusCode}.",
                statusCode,
                GetReason(statusCode));
        }
        return $"200 OK: data from {url}";
    }

    public Result<bool> PostData(string url, string data)
    {
        if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("http"))
        {
            return Result<bool>.Failure("Invalid URL: it must start with http.");
        }
        if (string.IsNullOrWhiteSpace(data))
        {
            return Result<bool>.Failure("POST data is empty.");
        }
        try
        {
            JsonDocument.Parse(data);
        }
        catch (JsonException ex)
        {
            return Result<bool>.Failure(ex);
        }
        return Result<bool>.Success(true);
    }

    private static int GetStatusCode(string url)
    {
        if (url.Contains("/forbidden"))
        {
            return 403;
        }
        if (url.Contains("/missing"))
        {
            return 404;
        }
        if (url.Contains("/crash"))
        {
            return 500;
        }
        return 200;
    }

    private static string GetReason(int statusCode)
    {
        return statusCode switch
        {
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }
}
