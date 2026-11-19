namespace lab12v9;

public class NetworkClient
{
    private int _attempts;

    public void Reset()
    {
        _attempts = 0;
    }

    public string GetRemoteSetting(string url, string key)
    {
        _attempts++;
        if (_attempts <= 3)
        {
            throw new HttpRequestException($"Cannot connect to {url}.");
        }
        return $"{key} = remote-value";
    }
}
