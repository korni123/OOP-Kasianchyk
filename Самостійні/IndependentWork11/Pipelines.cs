namespace IndependentWork11;

public class Pipelines
{
    private readonly List<Sale> _sales;
    private readonly List<Product> _products;
    private readonly List<Customer> _customers;

    public Pipelines(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        _sales = sales;
        _products = products;
        _customers = customers;
    }

    public List<string> TopProductsByCategory()
    {
        return _sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
            .Join(_products,
                  t => t.ProductId,
                  p => p.Id,
                  (t, p) => new { p.Name, p.Category, t.TotalQuantity })
            .GroupBy(x => x.Category)
            .OrderBy(g => g.Key)
            .SelectMany(g => g
                .OrderByDescending(x => x.TotalQuantity)
                .Take(3)
                .Select((x, i) => $"{g.Key}: {i + 1}. {x.Name} - {x.TotalQuantity} pcs"))
            .ToList();
    }

    public List<string> RevenueByCity()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(_customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.City, x.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.TotalRevenue)
            .Select(x => $"{x.City}: {x.TotalRevenue:N0} UAH")
            .ToList();
    }

    public List<string> SalesByMonth()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id, (s, p) => new { s.SaleDate, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
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
            .Select(x => $"{x.Year}-{x.Month:D2}: {x.TotalQuantity} pcs, {x.TotalRevenue:N0} UAH")
            .ToList();
    }

    public List<string> TopCustomersBySpending()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Spent = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new { CustomerId = g.Key, TotalSpent = g.Sum(x => x.Spent) })
            .Join(_customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.Name, c.City, x.TotalSpent })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .Select((x, i) => $"{i + 1}. {x.Name} ({x.City}): {x.TotalSpent:N0} UAH")
            .ToList();
    }
}
