using IndependentWork14;

RetryPolicy policy = new RetryPolicy(maxRetries: 4, initialDelay: TimeSpan.FromMilliseconds(300), jitterFactor: 0.2);

Console.WriteLine("=== Scenario 1: fails twice, then succeeds ===");
int calls = 0;
string answer = policy.Execute(() =>
{
    calls++;
    if (calls <= 2)
    {
        throw new TimeoutException("The server did not answer in time.");
    }
    return "Data received from the server";
});
Console.WriteLine("Result: " + answer);

Console.WriteLine();
Console.WriteLine("=== Scenario 2: always fails ===");
try
{
    policy.Execute<int>(() => throw new InvalidOperationException("The service is down."));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Exception after all attempts: " + ex.Message);
}

Console.WriteLine();
Console.WriteLine("=== Scenario 3: shouldRetry (only TimeoutException is retried) ===");
int calls3 = 0;
try
{
    policy.Execute<int>(() =>
    {
        calls3++;
        if (calls3 == 1)
        {
            throw new TimeoutException("Timeout.");
        }
        throw new ArgumentException("Wrong request, retry is pointless.");
    }, ex => ex is TimeoutException);
}
catch (ArgumentException ex)
{
    Console.WriteLine("Exception without more retries: " + ex.Message);
}
