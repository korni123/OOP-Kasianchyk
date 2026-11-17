using System.Collections.Concurrent;
using System.Diagnostics;

Console.WriteLine($"Processor cores: {Environment.ProcessorCount}");
Console.WriteLine();

List<string> tableRows = new List<string>();

List<int> CreateNumbers(int count)
{
    Random random = new Random(42);
    List<int> numbers = new List<int>(count);
    for (int i = 0; i < count; i++)
    {
        numbers.Add(random.Next(1, 10_000_000));
    }
    return numbers;
}

bool IsPrime(int n)
{
    if (n < 2)
    {
        return false;
    }
    if (n % 2 == 0)
    {
        return n == 2;
    }
    for (int i = 3; (long)i * i <= n; i += 2)
    {
        if (n % i == 0)
        {
            return false;
        }
    }
    return true;
}

void CompareHeavy(int count)
{
    List<int> numbers = CreateNumbers(count);

    Stopwatch sw = Stopwatch.StartNew();
    List<double> linqResult = numbers.Where(n => IsPrime(n)).Select(n => Math.Sqrt(n)).ToList();
    sw.Stop();
    double linqMs = sw.Elapsed.TotalMilliseconds;

    sw.Restart();
    List<double> plinqResult = numbers.AsParallel().Where(n => IsPrime(n)).Select(n => Math.Sqrt(n)).ToList();
    sw.Stop();
    double plinqMs = sw.Elapsed.TotalMilliseconds;

    Console.WriteLine($"Heavy operation (prime check), {count:N0} elements:");
    Console.WriteLine($"  LINQ:  {linqMs:F0} ms, found {linqResult.Count:N0}");
    Console.WriteLine($"  PLINQ: {plinqMs:F0} ms, found {plinqResult.Count:N0}");
    Console.WriteLine($"  PLINQ is {linqMs / plinqMs:F2} times faster than LINQ");
    Console.WriteLine();
    tableRows.Add($"| Prime check | {count:N0} | {linqMs:F0} | {plinqMs:F0} | {linqMs / plinqMs:F2} |");
}

void CompareCheap(int count)
{
    List<int> numbers = CreateNumbers(count);

    Stopwatch sw = Stopwatch.StartNew();
    List<int> linqResult = numbers.Where(n => n % 2 == 0).Select(n => n * 2).ToList();
    sw.Stop();
    double linqMs = sw.Elapsed.TotalMilliseconds;

    sw.Restart();
    List<int> plinqResult = numbers.AsParallel().Where(n => n % 2 == 0).Select(n => n * 2).ToList();
    sw.Stop();
    double plinqMs = sw.Elapsed.TotalMilliseconds;

    Console.WriteLine($"Cheap operation (even numbers), {count:N0} elements:");
    Console.WriteLine($"  LINQ:  {linqMs:F0} ms, found {linqResult.Count:N0}");
    Console.WriteLine($"  PLINQ: {plinqMs:F0} ms, found {plinqResult.Count:N0}");
    Console.WriteLine($"  PLINQ is {linqMs / plinqMs:F2} times faster than LINQ");
    Console.WriteLine();
    tableRows.Add($"| Even numbers | {count:N0} | {linqMs:F0} | {plinqMs:F0} | {linqMs / plinqMs:F2} |");
}

Console.WriteLine("=== Part 1: LINQ vs PLINQ, heavy operation ===");
CompareHeavy(1_000_000);
CompareHeavy(10_000_000);

Console.WriteLine("=== Part 2: LINQ vs PLINQ, cheap operation ===");
CompareCheap(1_000_000);
CompareCheap(10_000_000);

Console.WriteLine("=== Part 3: side effects in PLINQ ===");
List<int> data = CreateNumbers(1_000_000);
int expected = data.Count(n => n % 2 == 0);
Console.WriteLine($"Expected number of even elements: {expected:N0}");
Console.WriteLine();

Console.WriteLine("Problem: shared variable changed without synchronization (counter++)");
for (int run = 1; run <= 3; run++)
{
    int counter = 0;
    data.AsParallel().WithDegreeOfParallelism(4).ForAll(n =>
    {
        if (n % 2 == 0)
        {
            counter++;
        }
    });
    Console.WriteLine($"  Run {run}: counter = {counter:N0} (lost {expected - counter:N0})");
}

Console.WriteLine();
Console.WriteLine("Problem: List<int>.Add from several threads");
for (int run = 1; run <= 3; run++)
{
    List<int> evenList = new List<int>();
    try
    {
        data.AsParallel().WithDegreeOfParallelism(4).ForAll(n =>
        {
            if (n % 2 == 0)
            {
                evenList.Add(n);
            }
        });
        Console.WriteLine($"  Run {run}: list count = {evenList.Count:N0} (lost {expected - evenList.Count:N0})");
    }
    catch (AggregateException ex)
    {
        Console.WriteLine($"  Run {run}: exception {ex.InnerException?.GetType().Name}");
    }
}

Console.WriteLine();
Console.WriteLine("Fix 1: lock");
object locker = new object();
int lockCounter = 0;
data.AsParallel().WithDegreeOfParallelism(4).ForAll(n =>
{
    if (n % 2 == 0)
    {
        lock (locker)
        {
            lockCounter++;
        }
    }
});
Console.WriteLine($"  counter = {lockCounter:N0}");

Console.WriteLine("Fix 2: Interlocked.Increment");
int interlockedCounter = 0;
data.AsParallel().WithDegreeOfParallelism(4).ForAll(n =>
{
    if (n % 2 == 0)
    {
        Interlocked.Increment(ref interlockedCounter);
    }
});
Console.WriteLine($"  counter = {interlockedCounter:N0}");

Console.WriteLine("Fix 3: ConcurrentBag<int>");
ConcurrentBag<int> bag = new ConcurrentBag<int>();
data.AsParallel().WithDegreeOfParallelism(4).ForAll(n =>
{
    if (n % 2 == 0)
    {
        bag.Add(n);
    }
});
Console.WriteLine($"  bag count = {bag.Count:N0}");

Console.WriteLine("Fix 4: no shared state (PLINQ Count)");
int pureCount = data.AsParallel().Count(n => n % 2 == 0);
Console.WriteLine($"  count = {pureCount:N0}");

Console.WriteLine();
Console.WriteLine("=== Table for REPORT.md ===");
Console.WriteLine("| Operation | Elements | LINQ, ms | PLINQ, ms | Speedup |");
Console.WriteLine("|---|---|---|---|---|");
foreach (string row in tableRows)
{
    Console.WriteLine(row);
}
