using lab12v9;

FileProcessor fileProcessor = new FileProcessor();
NetworkClient networkClient = new NetworkClient();
TimeSpan delay = TimeSpan.FromMilliseconds(300);
Func<Exception, bool> shouldRetry = ex => ex is FileNotFoundException || ex is HttpRequestException;

Console.WriteLine("=== 1. FileProcessor.GetSetting: FileNotFoundException twice, then success ===");
string theme = RetryHelper.ExecuteWithRetry(
    () => fileProcessor.GetSetting("settings.ini", "theme"),
    retryCount: 3,
    initialDelay: delay,
    shouldRetry: shouldRetry);
Console.WriteLine("Result: theme = " + theme);

Console.WriteLine();
Console.WriteLine("=== 2. NetworkClient.GetRemoteSetting: HttpRequestException 3 times, then success ===");
string remote = RetryHelper.ExecuteWithRetry(
    () => networkClient.GetRemoteSetting("https://example.com/settings", "language"),
    retryCount: 3,
    initialDelay: delay,
    shouldRetry: shouldRetry);
Console.WriteLine("Result: " + remote);

Console.WriteLine();
Console.WriteLine("=== 3. Not enough retries: only 2 retries for a method that fails 3 times ===");
networkClient.Reset();
try
{
    RetryHelper.ExecuteWithRetry(
        () => networkClient.GetRemoteSetting("https://example.com/settings", "language"),
        retryCount: 2,
        initialDelay: delay,
        shouldRetry: shouldRetry);
}
catch (HttpRequestException ex)
{
    Console.WriteLine("Failed after all retries: " + ex.Message);
}

Console.WriteLine();
Console.WriteLine("=== 4. shouldRetry: UnauthorizedAccessException is not retried ===");
try
{
    RetryHelper.ExecuteWithRetry<string>(
        () => throw new UnauthorizedAccessException("No access to the settings file."),
        retryCount: 3,
        initialDelay: delay,
        shouldRetry: shouldRetry);
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine("Stopped at once: " + ex.Message);
}
