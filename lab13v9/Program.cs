using lab13v9;

ApiService service = new ApiService();

Console.WriteLine("=== MakeRequest: custom exception ApiRequestFailedException ===");
string[] urls =
{
    "https://api.example.com/users",
    "https://api.example.com/missing",
    "https://api.example.com/crash"
};
foreach (string url in urls)
{
    try
    {
        string response = service.MakeRequest(url);
        Console.WriteLine("OK: " + response);
    }
    catch (ApiRequestFailedException ex)
    {
        Console.WriteLine($"ApiRequestFailedException: {ex.Message}");
        Console.WriteLine($"  StatusCode: {ex.StatusCode}, ResponseContent: {ex.ResponseContent}");
    }
}

Console.WriteLine();
Console.WriteLine("=== PostData: Result<bool> ===");
(string Url, string Data)[] requests =
{
    ("https://api.example.com/users", "{\"name\": \"Oksana\", \"age\": 20}"),
    ("api.example.com/users", "{\"name\": \"Oksana\"}"),
    ("https://api.example.com/users", ""),
    ("https://api.example.com/users", "name = Oksana")
};
foreach ((string url, string data) in requests)
{
    Result<bool> result = service.PostData(url, data);
    if (result.IsSuccess)
    {
        Console.WriteLine($"Success: data posted to {url}, value = {result.Value}");
    }
    else
    {
        Console.WriteLine($"Failure: {result.Error}");
    }
}
