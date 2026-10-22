using System;
using System.Collections.Generic;
using System.Linq;

public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();
        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }
        return result;
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        T maxItem = default!;
        TKey maxKey = default!;
        bool first = true;

        foreach (var item in collection)
        {
            TKey key = keySelector(item);
            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
        {
            throw new InvalidOperationException("Колекція порожня.");
        }

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}

public class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== List<Product> ===");
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 25000, Category = "Electronics" },
            new Product { Name = "Mouse", Price = 500, Category = "Accessories" },
            new Product { Name = "Keyboard", Price = 1500, Category = "Accessories" },
            new Product { Name = "Monitor", Price = 7200, Category = "Electronics" },
            new Product { Name = "USB cable", Price = 150, Category = "Accessories" }
        };

        var names = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("Names: " + string.Join(", ", names));

        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Most expensive: {mostExpensive.Name} - {mostExpensive.Price}");

        var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
        Console.WriteLine("Expensive products (> 1000):");
        CollectionUtils.ForEach(expensive, p => Console.WriteLine($"  - {p.Name}"));

        Console.WriteLine("All products:");
        CollectionUtils.ForEach(products, p => Console.WriteLine($"  {p.Name}: {p.Price} грн ({p.Category})"));

        Console.WriteLine();
        Console.WriteLine("=== List<int> ===");
        var numbers = new List<int> { 7, -3, 12, 0, 25, 8, -10, 3 };
        Console.WriteLine("Numbers: " + string.Join(", ", numbers));
        Console.WriteLine("Map (x * x): " + string.Join(", ", CollectionUtils.Map(numbers, x => x * x)));
        Console.WriteLine("Map (x -> \"парне/непарне\"): " + string.Join(", ", CollectionUtils.Map(numbers, x => x % 2 == 0 ? "парне" : "непарне")));
        Console.WriteLine("Filter (x > 0): " + string.Join(", ", CollectionUtils.Filter(numbers, x => x > 0)));
        Console.WriteLine($"MaxBy (|x|): {CollectionUtils.MaxBy(numbers, x => Math.Abs(x))}");
        int sum = 0;
        CollectionUtils.ForEach(numbers, x => sum += x);
        Console.WriteLine($"ForEach (сума через замикання): {sum}");

        Console.WriteLine();
        Console.WriteLine("=== List<string> ===");
        var cities = new List<string> { "Рівне", "Київ", "Луцьк", "Івано-Франківськ", "Львів", "Ужгород" };
        Console.WriteLine("Cities: " + string.Join(", ", cities));
        Console.WriteLine("Map (ToUpper): " + string.Join(", ", CollectionUtils.Map(cities, c => c.ToUpper())));
        Console.WriteLine("Map (довжина): " + string.Join(", ", CollectionUtils.Map(cities, c => $"{c}={c.Length}")));
        Console.WriteLine("Filter (починається на 'Л'): " + string.Join(", ", CollectionUtils.Filter(cities, c => c.StartsWith("Л"))));
        Console.WriteLine($"MaxBy (найдовша назва): {CollectionUtils.MaxBy(cities, c => c.Length)}");
        Console.Write("ForEach (перші літери): ");
        CollectionUtils.ForEach(cities, c => Console.Write(c[0] + " "));
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("=== Порівняння з LINQ ===");
        bool sameSelect = CollectionUtils.Map(products, p => p.Name).SequenceEqual(products.Select(p => p.Name));
        bool sameWhere = CollectionUtils.Filter(products, p => p.Price > 1000).SequenceEqual(products.Where(p => p.Price > 1000));
        bool sameMax = CollectionUtils.MaxBy(products, p => p.Price) == products.MaxBy(p => p.Price);
        Console.WriteLine($"Map == Select: {sameSelect}");
        Console.WriteLine($"Filter == Where: {sameWhere}");
        Console.WriteLine($"MaxBy == Enumerable.MaxBy: {sameMax}");
    }
}
