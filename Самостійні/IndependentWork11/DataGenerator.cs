namespace IndependentWork11;

public static class DataGenerator
{
    private static readonly string[] Categories = { "Electronics", "Books", "Clothing", "Home", "Sports", "Toys" };
    private static readonly string[] ProductWords =
    {
        "Basic", "Pro", "Ultra", "Mini", "Max", "Classic", "Smart", "Eco", "Lite", "Prime"
    };
    private static readonly string[] Cities = { "Rivne", "Lviv", "Kyiv", "Kharkiv", "Odesa", "Dnipro", "Lutsk", "Ternopil" };
    private static readonly string[] FirstNames =
    {
        "Oleh", "Iryna", "Taras", "Sofiia", "Andrii", "Maryna", "Pavlo", "Oksana", "Yurii", "Halyna"
    };
    private static readonly string[] LastNames =
    {
        "Melnyk", "Shevchenko", "Boyko", "Kovalenko", "Bondarenko", "Tkachenko"
    };

    public static List<Product> CreateProducts(Random random, int count)
    {
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            string category = Categories[(i - 1) % Categories.Length];
            string name = $"{category} {ProductWords[random.Next(ProductWords.Length)]} {i}";
            decimal price = random.Next(50, 5000);
            products.Add(new Product(i, name, category, price));
        }
        return products;
    }

    public static List<Customer> CreateCustomers(Random random, int count)
    {
        var customers = new List<Customer>();
        for (int i = 1; i <= count; i++)
        {
            string name = $"{FirstNames[random.Next(FirstNames.Length)]} {LastNames[random.Next(LastNames.Length)]}";
            string city = Cities[random.Next(Cities.Length)];
            customers.Add(new Customer(i, name, city));
        }
        return customers;
    }

    public static List<Sale> CreateSales(Random random, int count, int productCount, int customerCount)
    {
        var sales = new List<Sale>();
        DateTime start = new DateTime(2025, 1, 1);
        for (int i = 1; i <= count; i++)
        {
            int productId = random.Next(1, productCount + 1);
            int customerId = random.Next(1, customerCount + 1);
            int quantity = random.Next(1, 11);
            DateTime date = start.AddDays(random.Next(0, 680));
            sales.Add(new Sale(i, productId, quantity, date, customerId));
        }
        return sales;
    }
}
