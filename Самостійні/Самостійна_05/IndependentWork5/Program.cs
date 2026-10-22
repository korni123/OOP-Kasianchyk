using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"Product: {Name}, ціна {Price} грн";
    }
}

public class Customer
{
    public string FullName { get; set; }
    public string City { get; set; }

    public Customer(string fullName, string city)
    {
        FullName = fullName;
        City = city;
    }

    public override string ToString()
    {
        return $"Customer: {FullName} ({City})";
    }
}

public class Order
{
    public int Number { get; set; }
    public Customer Customer { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public Order(int number, Customer customer, Product product, int quantity)
    {
        Number = number;
        Customer = customer;
        Product = product;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"Order #{Number}: {Customer.FullName} купує {Product.Name} x{Quantity} = {Product.Price * Quantity} грн";
    }
}

public class Note
{
    public string Text { get; set; }

    public Note(string text)
    {
        Text = text;
    }
}

public class MyCustomCollection : IEnumerable<string>
{
    private List<string> _items = new List<string>();

    public void Add(string item)
    {
        _items.Add(item);
    }

    public int Count
    {
        get { return _items.Count; }
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (string item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== 1. Поліморфізм через System.Object (ToString) ===");
        Customer customer = new Customer("Олена Коваль", "Рівне");
        Product product = new Product("Ноутбук", 32000);
        Order order = new Order(1, customer, product, 2);

        List<object> objects = new List<object> { product, customer, order, new Note("клас без override"), 42, "звичайний рядок" };
        foreach (object obj in objects)
        {
            Console.WriteLine($"{obj.GetType().Name,-10} -> {obj}");
        }

        Console.WriteLine();
        Console.WriteLine("=== 2. Поліморфізм через IEnumerable<string> ===");
        MyCustomCollection collection = new MyCustomCollection();
        collection.Add("C#");
        collection.Add("Java");
        collection.Add("Python");
        collection.Add("Go");

        foreach (string item in collection)
        {
            Console.WriteLine($"  елемент: {item}");
        }

        PrintAll("MyCustomCollection", collection);
        PrintAll("List<string>", new List<string> { "a", "b", "c" });
        PrintAll("string[]", new string[] { "x", "y" });

        Console.WriteLine();
        Console.WriteLine("=== 3. Поліморфізм через абстрактний клас Stream ===");
        string path = Path.Combine(AppContext.BaseDirectory, "test.txt");
        File.WriteAllText(path, "Привіт із FileStream! Це тестовий файл.");

        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            ProcessStream(fileStream);
        }

        byte[] bytes = Encoding.UTF8.GetBytes("Hello from MemoryStream!");
        using (MemoryStream memoryStream = new MemoryStream(bytes))
        {
            ProcessStream(memoryStream);
        }
    }

    static void PrintAll(string title, IEnumerable<string> items)
    {
        Console.WriteLine($"{title}: {string.Join(", ", items)}");
    }

    static void ProcessStream(Stream stream)
    {
        Console.WriteLine($"Тип потоку: {stream.GetType().Name}, довжина: {stream.Length} байт, CanSeek: {stream.CanSeek}");

        byte[] buffer = new byte[16];
        int read = stream.Read(buffer, 0, buffer.Length);
        Console.WriteLine($"  Прочитано {read} байт: {BitConverter.ToString(buffer, 0, read)}");

        stream.Position = 0;
        StreamReader reader = new StreamReader(stream);
        Console.WriteLine($"  Вміст: {reader.ReadToEnd()}");
    }
}
