using IndependentWork13;

Cache<string, string> cache = new Cache<string, string>(3);

Console.WriteLine("Cache<string, string>, capacity = 3");
cache.Add("a", "apple");
cache.Add("b", "banana");
cache.Add("c", "cherry");
Console.WriteLine("Added a, b, c. Count: " + cache.Count);
Console.WriteLine("Get a: " + cache.Get("a"));
Console.WriteLine("Get c: " + cache.Get("c"));

Console.WriteLine();
Console.WriteLine("Add d (cache is full, the oldest key 'a' must be removed)");
cache.Add("d", "date");
Console.WriteLine("Count: " + cache.Count);
Console.WriteLine("Contains a: " + cache.ContainsKey("a"));
Console.WriteLine("Contains b: " + cache.ContainsKey("b"));
Console.WriteLine("Contains d: " + cache.ContainsKey("d"));

Console.WriteLine();
Console.WriteLine("Add e (the oldest key 'b' must be removed)");
cache.Add("e", "elderberry");
Console.WriteLine("Contains b: " + cache.ContainsKey("b"));
Console.WriteLine("Contains c: " + cache.ContainsKey("c"));

Console.WriteLine();
Console.WriteLine("Update key d (order does not change)");
cache.Add("d", "dragon fruit");
Console.WriteLine("Get d: " + cache.Get("d"));
Console.WriteLine("Count: " + cache.Count);

Console.WriteLine();
Console.WriteLine("Get missing key 'x':");
try
{
    cache.Get("x");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.WriteLine();
Cache<int, double> numbers = new Cache<int, double>(2);
Console.WriteLine("Cache<int, double>, capacity = 2");
numbers.Add(1, 1.5);
numbers.Add(2, 2.5);
numbers.Add(3, 3.5);
Console.WriteLine("Contains 1: " + numbers.ContainsKey(1));
Console.WriteLine("Contains 3: " + numbers.ContainsKey(3));
Console.WriteLine("Count: " + numbers.Count);
