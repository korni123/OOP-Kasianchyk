namespace IndependentWork14;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new Random();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRetries));
        }
        if (jitterFactor < 0 || jitterFactor > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));
        }
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        int retryAttempt = 0;
        while (true)
        {
            Console.WriteLine($"  Attempt {retryAttempt + 1}...");
            try
            {
                T result = operation();
                Console.WriteLine($"  Attempt {retryAttempt + 1} succeeded.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Attempt {retryAttempt + 1} failed: {ex.GetType().Name}: {ex.Message}");

                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("  This error must not be retried. Giving up.");
                    throw;
                }
                if (retryAttempt >= _maxRetries)
                {
                    Console.WriteLine($"  All {_maxRetries} retries are used. Giving up.");
                    throw;
                }

                TimeSpan delay = CalculateDelay(retryAttempt);
                Console.WriteLine($"  Waiting {delay.TotalMilliseconds:F0} ms before retry {retryAttempt + 1} of {_maxRetries}.");
                Thread.Sleep(delay);
                retryAttempt++;
            }
        }
    }

    private TimeSpan CalculateDelay(int retryAttempt)
    {
        double jitter = 1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor;
        return _initialDelay * Math.Pow(2, retryAttempt) * jitter;
    }
}
