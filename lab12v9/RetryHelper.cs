namespace lab12v9;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (initialDelay == default)
        {
            initialDelay = TimeSpan.FromMilliseconds(500);
        }

        int retryAttempt = 0;
        while (true)
        {
            try
            {
                Console.WriteLine($"  Attempt {retryAttempt + 1}...");
                T result = operation();
                Console.WriteLine($"  Attempt {retryAttempt + 1} succeeded.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Attempt {retryAttempt + 1} failed: {ex.GetType().Name}: {ex.Message}");

                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("  This exception must not be retried.");
                    throw;
                }
                if (retryAttempt >= retryCount)
                {
                    Console.WriteLine($"  All {retryCount} retries are used.");
                    throw;
                }

                TimeSpan delay = initialDelay * Math.Pow(2, retryAttempt);
                Console.WriteLine($"  Waiting {delay.TotalMilliseconds:F0} ms (retry {retryAttempt + 1} of {retryCount})...");
                Thread.Sleep(delay);
                retryAttempt++;
            }
        }
    }
}
