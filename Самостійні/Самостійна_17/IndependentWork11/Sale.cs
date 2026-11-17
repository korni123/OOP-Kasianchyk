namespace IndependentWork11;

public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }

    public Sale(int id, int productId, int quantity, DateTime saleDate, int customerId)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        SaleDate = saleDate;
        CustomerId = customerId;
    }
}
