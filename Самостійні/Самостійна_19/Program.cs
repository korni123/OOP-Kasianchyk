using IndependentWork19;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

Console.WriteLine("=== Part 1: custom exception InsufficientFundsException ===");
BankAccount account = new BankAccount("Oksana", 1000m);

try
{
    account.Withdraw(300m);
    Console.WriteLine($"Withdrawn 300. Balance: {account.Balance}");
    account.Withdraw(5000m);
}
catch (InsufficientFundsException ex)
{
    Console.WriteLine($"InsufficientFundsException: {ex.Message}");
    Console.WriteLine($"  Requested: {ex.RequestedAmount}, balance: {ex.CurrentBalance}");
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine("Wrong amount: " + ex.Message);
}
finally
{
    Console.WriteLine("Operation finished.");
}

try
{
    account.Withdraw(-10m);
}
catch (InsufficientFundsException ex)
{
    Console.WriteLine("InsufficientFundsException: " + ex.Message);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine("ArgumentOutOfRangeException: " + ex.Message);
}

Console.WriteLine("Result-style method TryWithdraw (no exception):");
bool ok = account.TryWithdraw(9999m, out string error);
Console.WriteLine($"  success: {ok}, error: {error}");

Console.WriteLine();
Console.WriteLine("=== Scenario 1: Retry with exponential backoff (external exchange rate API) ===");
int apiCalls = 0;
bool apiAlwaysDown = false;

string CallExchangeApi()
{
    apiCalls++;
    if (apiAlwaysDown || apiCalls <= 2)
    {
        Console.WriteLine($"  Call {apiCalls}: HTTP 503 Service Unavailable");
        throw new HttpRequestException("Service unavailable (503)");
    }
    Console.WriteLine($"  Call {apiCalls}: OK");
    return "USD/UAH = 41.20";
}

ResiliencePipeline retryPipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromMilliseconds(500),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        OnRetry = args =>
        {
            Console.WriteLine($"  Retry #{args.AttemptNumber + 1} in {args.RetryDelay.TotalMilliseconds:F0} ms: {args.Outcome.Exception?.Message}");
            return default;
        }
    })
    .Build();

string rate = retryPipeline.Execute(() => CallExchangeApi());
Console.WriteLine("Result: " + rate);

Console.WriteLine();
Console.WriteLine("The API is down all the time:");
apiCalls = 0;
apiAlwaysDown = true;
try
{
    retryPipeline.Execute(() => CallExchangeApi());
}
catch (HttpRequestException ex)
{
    Console.WriteLine("Final failure after all attempts: " + ex.Message);
}

Console.WriteLine();
Console.WriteLine("=== Scenario 2: Circuit Breaker (database connection) ===");
bool databaseAvailable = false;

string QueryDatabase(int callNumber)
{
    if (!databaseAvailable)
    {
        Console.WriteLine($"  Call {callNumber}: connection to the database failed");
        throw new DatabaseConnectionException("Cannot connect to the database.");
    }
    Console.WriteLine($"  Call {callNumber}: query executed");
    return "42 rows";
}

ResiliencePipeline breakerPipeline = new ResiliencePipelineBuilder()
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<DatabaseConnectionException>(),
        FailureRatio = 1.0,
        MinimumThroughput = 3,
        SamplingDuration = TimeSpan.FromSeconds(10),
        BreakDuration = TimeSpan.FromSeconds(2),
        OnOpened = args =>
        {
            Console.WriteLine($"  [Breaker] OPEN: requests are rejected for {args.BreakDuration.TotalSeconds:F0} s");
            return default;
        },
        OnHalfOpened = args =>
        {
            Console.WriteLine("  [Breaker] HALF-OPEN: one test request is allowed");
            return default;
        },
        OnClosed = args =>
        {
            Console.WriteLine("  [Breaker] CLOSED: the service works again");
            return default;
        }
    })
    .Build();

for (int call = 1; call <= 7; call++)
{
    if (call == 6)
    {
        Console.WriteLine("  ... waiting 2.5 s, the database is repaired ...");
        await Task.Delay(2500);
        databaseAvailable = true;
    }
    try
    {
        int current = call;
        string result = breakerPipeline.Execute(() => QueryDatabase(current));
        Console.WriteLine($"  Call {call} result: {result}");
    }
    catch (BrokenCircuitException)
    {
        Console.WriteLine($"  Call {call}: rejected at once, the circuit is open (the database was not called)");
    }
    catch (DatabaseConnectionException ex)
    {
        Console.WriteLine($"  Call {call} failed: {ex.Message}");
    }
}

Console.WriteLine();
Console.WriteLine("=== Scenario 3: Timeout + Fallback (message queue) ===");

ResiliencePipeline<string> queuePipeline = new ResiliencePipelineBuilder<string>()
    .AddFallback(new FallbackStrategyOptions<string>
    {
        ShouldHandle = new PredicateBuilder<string>().Handle<TimeoutRejectedException>(),
        FallbackAction = args => Outcome.FromResultAsValueTask("Message saved to local storage"),
        OnFallback = args =>
        {
            Console.WriteLine("  [Fallback] the queue is too slow, using local storage");
            return default;
        }
    })
    .AddTimeout(TimeSpan.FromMilliseconds(500))
    .Build();

async Task SendMessage(int number, int queueDelayMs)
{
    Console.WriteLine($"  Message {number}: the queue needs {queueDelayMs} ms");
    string result = await queuePipeline.ExecuteAsync(async token =>
    {
        await Task.Delay(queueDelayMs, token);
        return "Message sent to the queue";
    });
    Console.WriteLine($"  Message {number} result: {result}");
}

await SendMessage(1, 100);
await SendMessage(2, 3000);
await SendMessage(3, 200);
