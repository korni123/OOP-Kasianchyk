using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine($"Логічних процесорів: {Environment.ProcessorCount}");
        Console.WriteLine();

        Warmup();
        PerformanceExperiments();
        SideEffectsDemo();
    }

    static List<int> CreateData(int size, int seed)
    {
        Random random = new Random(seed);
        List<int> data = new List<int>(size);
        for (int i = 0; i < size; i++)
        {
            data.Add(random.Next(1, 10_000_000));
        }
        return data;
    }

    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;
        int limit = (int)Math.Sqrt(n);
        for (int i = 3; i <= limit; i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    static double HeavyMath(int x)
    {
        double result = x;
        for (int i = 1; i <= 40; i++)
        {
            result = Math.Sqrt(Math.Abs(result) + i) * Math.Sin(result) + Math.Log(i + 1);
        }
        return result;
    }

    static void Warmup()
    {
        List<int> small = CreateData(10_000, 1);
        small.Where(x => x % 2 == 0).Select(x => (long)x * 2).ToList();
        small.AsParallel().Where(x => x % 2 == 0).Select(x => (long)x * 2).ToList();
        small.Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
        small.AsParallel().Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
        small.Where(x => x % 3 == 0).Select(HeavyMath).ToList();
        small.AsParallel().Where(x => x % 3 == 0).Select(HeavyMath).ToList();
    }

    static void PerformanceExperiments()
    {
        Console.WriteLine("=== Частина 1: LINQ vs PLINQ ===");
        Console.WriteLine($"{"Розмір",10} | {"Операція",-28} | {"LINQ, мс",10} | {"PLINQ, мс",10} | {"Прискорення",11} | {"Результатів",11}");
        Console.WriteLine(new string('-', 95));

        int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };
        foreach (int size in sizes)
        {
            List<int> data = CreateData(size, 42);

            Measure(size, "Легка: x*2 для парних",
                () => data.Where(x => x % 2 == 0).Select(x => (long)x * 2).ToList().Count,
                () => data.AsParallel().Where(x => x % 2 == 0).Select(x => (long)x * 2).ToList().Count);

            Measure(size, "Середня: перевірка на просте",
                () => data.Where(IsPrime).Select(x => Math.Sqrt(x)).ToList().Count,
                () => data.AsParallel().Where(IsPrime).Select(x => Math.Sqrt(x)).ToList().Count);

            Measure(size, "Важка: Sqrt/Sin/Log x40",
                () => data.Where(x => x % 3 == 0).Select(HeavyMath).ToList().Count,
                () => data.AsParallel().Where(x => x % 3 == 0).Select(HeavyMath).ToList().Count);

            Console.WriteLine(new string('-', 95));
        }
        Console.WriteLine();
    }

    static void Measure(int size, string operation, Func<int> linq, Func<int> plinq)
    {
        Stopwatch sw = Stopwatch.StartNew();
        int linqCount = linq();
        sw.Stop();
        double linqMs = sw.Elapsed.TotalMilliseconds;

        sw.Restart();
        int plinqCount = plinq();
        sw.Stop();
        double plinqMs = sw.Elapsed.TotalMilliseconds;

        string check = linqCount == plinqCount ? linqCount.ToString() : "РІЗНІ!";
        Console.WriteLine($"{size,10:N0} | {operation,-28} | {linqMs,10:F1} | {plinqMs,10:F1} | {linqMs / plinqMs,10:F2}x | {check,11}");
    }

    static void SideEffectsDemo()
    {
        Console.WriteLine("=== Частина 2: Побічні ефекти в PLINQ ===");
        List<int> data = CreateData(5_000_000, 7);
        int expected = data.Count(x => x % 2 == 0);
        Console.WriteLine($"Колекція: {data.Count:N0} елементів, очікувана кількість парних: {expected:N0}");
        Console.WriteLine();

        Console.WriteLine("1) НЕБЕЗПЕЧНО: counter++ зі спільною змінною всередині PLINQ");
        for (int run = 1; run <= 5; run++)
        {
            int counter = 0;
            data.AsParallel().ForAll(x =>
            {
                if (x % 2 == 0) counter++;
            });
            Console.WriteLine($"   Запуск {run}: counter = {counter,10:N0}  {(counter == expected ? "OK" : $"ПОМИЛКА, втрачено {expected - counter:N0}")}");
        }

        Console.WriteLine();
        Console.WriteLine("2) НЕБЕЗПЕЧНО: List<int>.Add зі спільним списком всередині PLINQ");
        for (int run = 1; run <= 3; run++)
        {
            List<int> unsafeList = new List<int>();
            try
            {
                data.AsParallel().Where(x => x % 2 == 0).ForAll(x => unsafeList.Add(x));
                Console.WriteLine($"   Запуск {run}: у списку {unsafeList.Count,10:N0}  {(unsafeList.Count == expected ? "OK" : $"ПОМИЛКА, втрачено {expected - unsafeList.Count:N0}")}");
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"   Запуск {run}: виняток {ex.InnerExceptions[0].GetType().Name} - список зламався під час паралельного Add");
            }
        }

        Console.WriteLine();
        Console.WriteLine("3) ВИПРАВЛЕННЯ");
        Stopwatch sw = Stopwatch.StartNew();
        int lockCounter = 0;
        object locker = new object();
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0)
            {
                lock (locker)
                {
                    lockCounter++;
                }
            }
        });
        sw.Stop();
        PrintFix("lock", lockCounter, expected, sw.Elapsed.TotalMilliseconds);

        sw.Restart();
        int interlockedCounter = 0;
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0) Interlocked.Increment(ref interlockedCounter);
        });
        sw.Stop();
        PrintFix("Interlocked.Increment", interlockedCounter, expected, sw.Elapsed.TotalMilliseconds);

        sw.Restart();
        ConcurrentBag<int> bag = new ConcurrentBag<int>();
        data.AsParallel().Where(x => x % 2 == 0).ForAll(x => bag.Add(x));
        sw.Stop();
        PrintFix("ConcurrentBag<int>", bag.Count, expected, sw.Elapsed.TotalMilliseconds);

        sw.Restart();
        int plinqCount = data.AsParallel().Count(x => x % 2 == 0);
        sw.Stop();
        PrintFix("без побічних ефектів: .Count()", plinqCount, expected, sw.Elapsed.TotalMilliseconds);

        sw.Restart();
        List<int> evens = data.AsParallel().Where(x => x % 2 == 0).ToList();
        sw.Stop();
        PrintFix("без побічних ефектів: .ToList()", evens.Count, expected, sw.Elapsed.TotalMilliseconds);
    }

    static void PrintFix(string method, int actual, int expected, double ms)
    {
        Console.WriteLine($"   {method,-32} -> {actual,10:N0}  {(actual == expected ? "OK" : "ПОМИЛКА")}  ({ms:F1} мс)");
    }
}
