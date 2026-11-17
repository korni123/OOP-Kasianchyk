namespace lab9v9;

public class BookingSystem
{
    private readonly HashSet<string> _clients = new HashSet<string>();
    private readonly Queue<Booking> _bookingQueue = new Queue<Booking>();
    private readonly Stack<Booking> _cancelledHistory = new Stack<Booking>();

    public int ClientsCount => _clients.Count;
    public int QueueCount => _bookingQueue.Count;
    public int CancelledCount => _cancelledHistory.Count;

    public bool AddClient(string name)
    {
        return _clients.Add(name);
    }

    public bool ContainsClient(string name)
    {
        return _clients.Contains(name);
    }

    public bool RemoveClient(string name)
    {
        return _clients.Remove(name);
    }

    public HashSet<string> GetClients()
    {
        return new HashSet<string>(_clients);
    }

    public void AddBooking(Booking booking)
    {
        _bookingQueue.Enqueue(booking);
        _clients.Add(booking.ClientName);
    }

    public Booking PeekNextBooking()
    {
        return _bookingQueue.Peek();
    }

    public Booking ProcessNextBooking()
    {
        return _bookingQueue.Dequeue();
    }

    public Booking CancelNextBooking()
    {
        Booking booking = _bookingQueue.Dequeue();
        _cancelledHistory.Push(booking);
        return booking;
    }

    public Booking PeekLastCancelled()
    {
        return _cancelledHistory.Peek();
    }

    public Booking RestoreLastCancelled()
    {
        Booking booking = _cancelledHistory.Pop();
        _bookingQueue.Enqueue(booking);
        return booking;
    }
}
