using System;
using System.Globalization;

public class Product
{
    private int _id;
    private string _name;
    private decimal _price;
    private string _category;
    private int _stockCount;

    public int Id
    {
        get { return _id; }
    }

    public string Name
    {
        get { return _name; }
    }

    public decimal Price
    {
        get { return _price; }
    }

    public string Category
    {
        get { return _category; }
    }

    public int StockCount
    {
        get { return _stockCount; }
    }

    public Product(int id, string name, decimal price, string category, int stockCount)
    {
        _id = id;
        _name = name;
        _price = price;
        _category = category;
        _stockCount = stockCount;
    }

    public Product(int id, string name, decimal price)
        : this(id, name, price, "Uncategorized", 0)
    {
    }

    public Product(Product other)
        : this(other.Id, other.Name, other.Price, other.Category, other.StockCount)
    {
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price:C}, Category: {Category}, Stock: {StockCount}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

        Console.WriteLine("Створення товарів");

        Product laptop = new Product(101, "Laptop", 35000m, "Electronics", 12);
        Console.WriteLine($"Товар 1 (основний конструктор): {laptop}");

        Product mouse = new Product(102, "Mouse", 800m);
        Console.WriteLine($"Товар 2 (скорочений конструктор): {mouse}");

        Product laptopCopy = new Product(laptop);
        Console.WriteLine($"Товар 3 (конструктор копіювання): {laptopCopy}");

        Console.WriteLine();
        Console.WriteLine($"laptop і laptopCopy - один і той самий об'єкт? {ReferenceEquals(laptop, laptopCopy)}");
    }
}
