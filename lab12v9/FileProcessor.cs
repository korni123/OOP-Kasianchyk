namespace lab12v9;

public class FileProcessor
{
    private readonly Dictionary<string, string> _settings = new Dictionary<string, string>
    {
        { "theme", "dark" },
        { "language", "uk" }
    };
    private int _attempts;

    public void Reset()
    {
        _attempts = 0;
    }

    public string GetSetting(string path, string key)
    {
        _attempts++;
        if (_attempts <= 2)
        {
            throw new FileNotFoundException($"Settings file '{path}' was not found.", path);
        }
        return _settings.TryGetValue(key, out string? value) ? value : "";
    }
}
