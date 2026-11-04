using lab8v9;

List<int> numbers = new List<int> { 7, 3, 9, 14, 5, 20, 8 };
List<string> words = new List<string> { "banana", "apple", "cherry", "avocado" };
List<Product> products = new List<Product>
{
    new Product(1, "Mouse", "Accessories", 350m),
    new Product(2, "Keyboard", "Accessories", 480m),
    new Product(3, "Monitor", "Displays", 6200m),
    new Product(4, "Laptop", "Computers", 28500m)
};

Console.WriteLine("List<int>: " + string.Join(", ", numbers));
int firstEven = CollectionHelper<int>.First(numbers, x => x % 2 == 0);
Console.WriteLine("First even number: " + firstEven);
int firstBig = CollectionHelper<int>.First(numbers, x => x > 15);
Console.WriteLine("First number > 15: " + firstBig);

Console.WriteLine();
Console.WriteLine("List<string>: " + string.Join(", ", words));
string firstA = CollectionHelper<string>.First(words, s => s.StartsWith("a"));
Console.WriteLine("First word starting with 'a': " + firstA);
string firstLong = CollectionHelper<string>.First(words, s => s.Length > 6);
Console.WriteLine("First word longer than 6 letters: " + firstLong);

Console.WriteLine();
Console.WriteLine("List<Product>:");
foreach (Product p in products)
{
    Console.WriteLine(p);
}
Product firstExpensive = CollectionHelper<Product>.First(products, p => p.Price > 500);
Console.WriteLine("First product with price > 500: " + firstExpensive);
Product firstComputer = CollectionHelper<Product>.First(products, p => p.Category == "Computers");
Console.WriteLine("First product in category Computers: " + firstComputer);

Console.WriteLine();
Console.WriteLine("No element found:");
try
{
    CollectionHelper<int>.First(numbers, x => x > 100);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Error (int): " + ex.Message);
}
try
{
    CollectionHelper<Product>.First(products, p => p.Price > 100000);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Error (Product): " + ex.Message);
}
