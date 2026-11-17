namespace IndependentWork11;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal UnitPrice { get; set; }

    public Product(int id, string name, string category, decimal unitPrice)
    {
        Id = id;
        Name = name;
        Category = category;
        UnitPrice = unitPrice;
    }
}
