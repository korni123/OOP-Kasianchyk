using IndependentWork15;

var numbers = new List<int> { 5, 12, 7, 20, 3, 18, 9 };
var words = new List<string> { "apple", "banana", "kiwi", "watermelon", "fig" };
var products = new List<Product>
{
    new Product("Laptop", 25000),
    new Product("Mouse", 500),
    new Product("Keyboard", 1500),
    new Product("Monitor", 7200)
};

Console.WriteLine("=== List<int> ===");
Console.WriteLine("Numbers: " + string.Join(", ", numbers));
var squares = CollectionUtils.Map(numbers, n => n * n);
Console.WriteLine("Map (squares): " + string.Join(", ", squares));
var texts = CollectionUtils.Map(numbers, n => "#" + n);
Console.WriteLine("Map (to string): " + string.Join(", ", texts));
Console.WriteLine("MaxBy (largest number): " + CollectionUtils.MaxBy(numbers, n => n));
Console.WriteLine("MaxBy (largest remainder of division by 5): " + CollectionUtils.MaxBy(numbers, n => n % 5));
var even = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("Filter (even): " + string.Join(", ", even));
var big = CollectionUtils.Filter(numbers, n => n > 10);
Console.WriteLine("Filter (> 10): " + string.Join(", ", big));
int sum = 0;
CollectionUtils.ForEach(numbers, n => sum += n);
Console.WriteLine("ForEach (sum): " + sum);

Console.WriteLine();
Console.WriteLine("=== List<string> ===");
Console.WriteLine("Words: " + string.Join(", ", words));
var lengths = CollectionUtils.Map(words, w => w.Length);
Console.WriteLine("Map (lengths): " + string.Join(", ", lengths));
var upper = CollectionUtils.Map(words, w => w.ToUpper());
Console.WriteLine("Map (upper case): " + string.Join(", ", upper));
Console.WriteLine("MaxBy (longest word): " + CollectionUtils.MaxBy(words, w => w.Length));
Console.WriteLine("MaxBy (last in alphabet): " + CollectionUtils.MaxBy(words, w => w));
var withA = CollectionUtils.Filter(words, w => w.Contains('a'));
Console.WriteLine("Filter (contains 'a'): " + string.Join(", ", withA));
var shortWords = CollectionUtils.Filter(words, w => w.Length <= 4);
Console.WriteLine("Filter (length <= 4): " + string.Join(", ", shortWords));
Console.WriteLine("ForEach (print with number):");
int index = 1;
CollectionUtils.ForEach(words, w => Console.WriteLine($"  {index++}. {w}"));

Console.WriteLine();
Console.WriteLine("=== List<Product> ===");
var names = CollectionUtils.Map(products, p => p.Name);
Console.WriteLine("Map (names): " + string.Join(", ", names));
var prices = CollectionUtils.Map(products, p => p.Price);
Console.WriteLine("Map (prices): " + string.Join(", ", prices));
var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"MaxBy (most expensive): {mostExpensive.Name} - {mostExpensive.Price}");
var longestName = CollectionUtils.MaxBy(products, p => p.Name.Length);
Console.WriteLine($"MaxBy (longest name): {longestName.Name}");
var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
Console.WriteLine("Filter (price > 1000):");
CollectionUtils.ForEach(expensive, p => Console.WriteLine($"  - {p.Name}"));
Console.WriteLine("ForEach (all products):");
CollectionUtils.ForEach(products, p => Console.WriteLine($"  {p.Name}: {p.Price} UAH"));
Console.WriteLine("ForEach (raise price by 10%):");
CollectionUtils.ForEach(products, p => p.Price = p.Price * 1.1m);
CollectionUtils.ForEach(products, p => Console.WriteLine($"  {p.Name}: {p.Price:F2} UAH"));
