using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal UnitPrice { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
}

public static class DataGenerator
{
    public static List<Product> Products = new List<Product>();
    public static List<Customer> Customers = new List<Customer>();
    public static List<Sale> Sales = new List<Sale>();

    public static void Generate(int productCount, int customerCount, int saleCount, int seed)
    {
        Random random = new Random(seed);

        Dictionary<string, string[]> catalog = new Dictionary<string, string[]>
        {
            ["Електроніка"] = new[] { "Ноутбук", "Смартфон", "Планшет", "Монітор", "Навушники", "Колонка" },
            ["Аксесуари"] = new[] { "Мишка", "Клавіатура", "Чохол", "Кабель USB-C", "Зарядка", "Флешка" },
            ["Книги"] = new[] { "Роман", "Підручник C#", "Детектив", "Комікс", "Енциклопедія", "Поезія" },
            ["Одяг"] = new[] { "Футболка", "Худі", "Джинси", "Кросівки", "Куртка", "Шапка" },
            ["Дім"] = new[] { "Лампа", "Чашка", "Плед", "Подушка", "Чайник", "Годинник" },
            ["Спорт"] = new[] { "М'яч", "Гантелі", "Килимок", "Скакалка", "Ракетка", "Пляшка" }
        };
        Dictionary<string, (int min, int max)> priceRange = new Dictionary<string, (int, int)>
        {
            ["Електроніка"] = (2000, 40000),
            ["Аксесуари"] = (100, 2500),
            ["Книги"] = (150, 900),
            ["Одяг"] = (300, 4000),
            ["Дім"] = (200, 3000),
            ["Спорт"] = (150, 3500)
        };
        string[] brands = { "Nova", "Prime", "Lite", "Pro", "Max", "Eco", "Smart", "Air", "Go", "One" };
        string[] cities = { "Рівне", "Київ", "Львів", "Луцьк", "Одеса", "Харків", "Дніпро", "Житомир" };
        string[] firstNames = { "Олена", "Андрій", "Марія", "Іван", "Софія", "Дмитро", "Анна", "Максим", "Ірина", "Богдан" };
        string[] lastNames = { "Коваль", "Шевчук", "Бондар", "Мельник", "Ткаченко", "Кравець", "Лисенко", "Савчук" };

        List<string> categories = catalog.Keys.ToList();
        for (int i = 1; i <= productCount; i++)
        {
            string category = categories[(i - 1) % categories.Count];
            string baseName = catalog[category][random.Next(catalog[category].Length)];
            (int min, int max) = priceRange[category];
            Products.Add(new Product
            {
                Id = i,
                Name = $"{baseName} {brands[random.Next(brands.Length)]}-{i}",
                Category = category,
                UnitPrice = random.Next(min, max) / 10 * 10m
            });
        }

        for (int i = 1; i <= customerCount; i++)
        {
            Customers.Add(new Customer
            {
                Id = i,
                Name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}",
                City = cities[random.Next(cities.Length)]
            });
        }

        DateTime start = new DateTime(2025, 1, 1);
        int days = (new DateTime(2026, 6, 30) - start).Days;
        for (int i = 1; i <= saleCount; i++)
        {
            Sales.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, productCount + 1),
                CustomerId = random.Next(1, customerCount + 1),
                Quantity = random.Next(1, 6),
                SaleDate = start.AddDays(random.Next(days + 1))
            });
        }
    }
}

public class Program
{
    static List<Product> products = new List<Product>();
    static List<Customer> customers = new List<Customer>();
    static List<Sale> sales = new List<Sale>();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

        DataGenerator.Generate(60, 80, 600, 42);
        products = DataGenerator.Products;
        customers = DataGenerator.Customers;
        sales = DataGenerator.Sales;
        Console.WriteLine($"Згенеровано: товарів - {products.Count}, клієнтів - {customers.Count}, продажів - {sales.Count} (Random seed = 42)");
        Console.WriteLine();

        double t1 = TopProductsByCategory(true);
        double t2 = RevenueByCity(true);
        double t3 = SalesByYearAndMonth(true);
        double t4 = TopCustomersWithFavoriteCategory(true);

        Console.WriteLine("=== Підсумок вимірювань (мс) ===");
        Console.WriteLine($"{"Пайплайн",-40} {"1-й запуск",12} {"після прогріву",16}");
        Console.WriteLine($"{"1. Топ-3 продукти за категоріями",-40} {t1,12:F3} {Warm(TopProductsByCategory),16:F3}");
        Console.WriteLine($"{"2. Дохід за містами",-40} {t2,12:F3} {Warm(RevenueByCity),16:F3}");
        Console.WriteLine($"{"3. Продажі за роками/місяцями",-40} {t3,12:F3} {Warm(SalesByYearAndMonth),16:F3}");
        Console.WriteLine($"{"4. Топ-5 клієнтів + улюблена категорія",-40} {t4,12:F3} {Warm(TopCustomersWithFavoriteCategory),16:F3}");
    }

    static double Warm(Func<bool, double> pipeline)
    {
        double total = 0;
        int runs = 100;
        for (int i = 0; i < runs; i++)
        {
            total += pipeline(false);
        }
        return total / runs;
    }

    static double TopProductsByCategory(bool print)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
            .Join(products,
                  t => t.ProductId,
                  p => p.Id,
                  (t, p) => new { p.Name, p.Category, t.TotalQuantity })
            .GroupBy(x => x.Category)
            .Select(g => new
            {
                Category = g.Key,
                Top = g.OrderByDescending(x => x.TotalQuantity).ThenBy(x => x.Name).Take(3).ToList()
            })
            .OrderBy(x => x.Category)
            .ToList();

        sw.Stop();

        if (print)
        {
            Console.WriteLine("=== Пайплайн 1: Найпопулярніші продукти за категоріями (топ-3) ===");
            foreach (var category in result)
            {
                Console.WriteLine($"{category.Category}:");
                int place = 1;
                foreach (var item in category.Top)
                {
                    Console.WriteLine($"  {place++}. {item.Name} - {item.TotalQuantity} шт.");
                }
            }
            Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine();
        }

        return sw.Elapsed.TotalMilliseconds;
    }

    static double RevenueByCity(bool print)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.City, x.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue), SalesCount = g.Count() })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        sw.Stop();

        if (print)
        {
            Console.WriteLine("=== Пайплайн 2: Загальний дохід за містами клієнтів ===");
            foreach (var city in result)
            {
                Console.WriteLine($"{city.City,-10} {city.TotalRevenue,15:N2} грн  ({city.SalesCount} продажів)");
            }
            Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine();
        }

        return sw.Elapsed.TotalMilliseconds;
    }

    static double SalesByYearAndMonth(bool print)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.SaleDate, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        sw.Stop();

        if (print)
        {
            Console.WriteLine("=== Пайплайн 3: Продажі за місяцями та роками ===");
            foreach (var month in result)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month);
                Console.WriteLine($"{month.Year} {monthName,-9} - {month.TotalQuantity,4} од., дохід {month.TotalRevenue,13:N2} грн");
            }
            Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine();
        }

        return sw.Elapsed.TotalMilliseconds;
    }

    static double TopCustomersWithFavoriteCategory(bool print)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, p.Category, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                TotalSpent = g.Sum(x => x.Revenue),
                Purchases = g.Count(),
                FavoriteCategory = g.GroupBy(x => x.Category)
                                    .Select(c => new { Category = c.Key, Qty = c.Sum(x => x.Quantity) })
                                    .Aggregate((best, next) => next.Qty > best.Qty ? next : best)
                                    .Category
            })
            .Join(customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.Name, c.City, x.TotalSpent, x.Purchases, x.FavoriteCategory })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToList();

        sw.Stop();

        if (print)
        {
            Console.WriteLine("=== Пайплайн 4 (власний): Топ-5 клієнтів за витратами та їхня улюблена категорія ===");
            int place = 1;
            foreach (var c in result)
            {
                Console.WriteLine($"{place++}. {c.Name} ({c.City}) - {c.TotalSpent:N2} грн за {c.Purchases} покупок, улюблена категорія: {c.FavoriteCategory}");
            }
            Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine();
        }

        return sw.Elapsed.TotalMilliseconds;
    }
}
