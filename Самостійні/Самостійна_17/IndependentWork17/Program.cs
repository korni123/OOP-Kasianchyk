using System.Diagnostics;
using IndependentWork17;

Random random = new Random(42);
List<Product> products = DataGenerator.CreateProducts(random, 60);
List<Customer> customers = DataGenerator.CreateCustomers(random, 80);
List<Sale> sales = DataGenerator.CreateSales(random, 500, products.Count, customers.Count);

Console.WriteLine($"Products: {products.Count}, customers: {customers.Count}, sales: {sales.Count}");
Console.WriteLine();

Pipelines pipelines = new Pipelines(sales, products, customers);

void Measure(string title, Func<List<string>> pipeline)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    List<string> result = pipeline();
    stopwatch.Stop();

    Console.WriteLine(title);
    foreach (string line in result)
    {
        Console.WriteLine("  " + line);
    }
    Console.WriteLine($"  Time: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
    Console.WriteLine();
}

Measure("Pipeline 1: Top 3 products by category", pipelines.TopProductsByCategory);
Measure("Pipeline 2: Revenue by customer city", pipelines.RevenueByCity);
Measure("Pipeline 3: Sales by year and month", pipelines.SalesByMonth);
Measure("Pipeline 4 (extra): Top 5 customers by spending", pipelines.TopCustomersBySpending);
