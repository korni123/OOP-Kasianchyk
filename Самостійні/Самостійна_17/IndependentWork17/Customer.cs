namespace IndependentWork17;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    public Customer(int id, string name, string city)
    {
        Id = id;
        Name = name;
        City = city;
    }
}
