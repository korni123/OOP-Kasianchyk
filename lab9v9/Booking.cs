namespace lab9v9;

public class Booking
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public int Room { get; set; }
    public DateTime Date { get; set; }

    public Booking(int id, string clientName, int room, DateTime date)
    {
        Id = id;
        ClientName = clientName;
        Room = room;
        Date = date;
    }

    public override string ToString()
    {
        return $"Booking #{Id}: {ClientName}, room {Room}, {Date:dd.MM.yyyy}";
    }
}
