namespace lab7v9;

public class Project : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Project(int id, string name, DateTime startDate, DateTime endDate)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public override string ToString()
    {
        return $"Project #{Id}: {Name}, {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";
    }
}
