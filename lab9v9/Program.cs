using System.Diagnostics;
using lab9v9;

BookingSystem system = new BookingSystem();

Console.WriteLine("=== HashSet: unique clients ===");
Console.WriteLine("Add Ivan: " + system.AddClient("Ivan"));
Console.WriteLine("Add Olena: " + system.AddClient("Olena"));
Console.WriteLine("Add Ivan again: " + system.AddClient("Ivan"));
Console.WriteLine("Add Petro: " + system.AddClient("Petro"));
Console.WriteLine("Clients count: " + system.ClientsCount);
Console.WriteLine("Contains Olena: " + system.ContainsClient("Olena"));
Console.WriteLine("Remove Petro: " + system.RemoveClient("Petro"));
Console.WriteLine("Contains Petro: " + system.ContainsClient("Petro"));

HashSet<string> vip = new HashSet<string> { "Olena", "Maria" };
HashSet<string> union = system.GetClients();
union.UnionWith(vip);
Console.WriteLine("UnionWith VIP: " + string.Join(", ", union));
HashSet<string> intersect = system.GetClients();
intersect.IntersectWith(vip);
Console.WriteLine("IntersectWith VIP: " + string.Join(", ", intersect));
HashSet<string> except = system.GetClients();
except.ExceptWith(vip);
Console.WriteLine("ExceptWith VIP: " + string.Join(", ", except));

Console.WriteLine();
Console.WriteLine("=== Queue: booking queue (FIFO) ===");
system.AddBooking(new Booking(1, "Ivan", 101, new DateTime(2026, 12, 1)));
system.AddBooking(new Booking(2, "Olena", 205, new DateTime(2026, 12, 3)));
system.AddBooking(new Booking(3, "Maria", 110, new DateTime(2026, 12, 5)));
system.AddBooking(new Booking(4, "Dmytro", 301, new DateTime(2026, 12, 7)));
Console.WriteLine("Queue count: " + system.QueueCount);
Console.WriteLine("Peek (next): " + system.PeekNextBooking());
Console.WriteLine("Process: " + system.ProcessNextBooking());
Console.WriteLine("Process: " + system.ProcessNextBooking());
Console.WriteLine("Queue count: " + system.QueueCount);
Console.WriteLine("Clients count after bookings: " + system.ClientsCount);

Console.WriteLine();
Console.WriteLine("=== Stack: cancelled bookings (LIFO) ===");
Console.WriteLine("Cancel: " + system.CancelNextBooking());
Console.WriteLine("Cancel: " + system.CancelNextBooking());
Console.WriteLine("Cancelled count: " + system.CancelledCount);
Console.WriteLine("Peek (last cancelled): " + system.PeekLastCancelled());
Console.WriteLine("Restore: " + system.RestoreLastCancelled());
Console.WriteLine("Cancelled count: " + system.CancelledCount);
Console.WriteLine("Queue count: " + system.QueueCount);

Console.WriteLine();
Console.WriteLine("=== Performance: List vs HashSet (Contains) ===");
int size = 50000;
int lookups = 5000;
List<int> list = new List<int>();
HashSet<int> set = new HashSet<int>();
for (int i = 0; i < size; i++)
{
    list.Add(i);
    set.Add(i);
}

int found = 0;
Stopwatch sw = Stopwatch.StartNew();
for (int i = 0; i < lookups; i++)
{
    if (list.Contains(size - 1 - i))
    {
        found++;
    }
}
sw.Stop();
Console.WriteLine($"List.Contains: {sw.Elapsed.TotalMilliseconds:F3} ms (found {found})");

found = 0;
sw.Restart();
for (int i = 0; i < lookups; i++)
{
    if (set.Contains(size - 1 - i))
    {
        found++;
    }
}
sw.Stop();
Console.WriteLine($"HashSet.Contains: {sw.Elapsed.TotalMilliseconds:F3} ms (found {found})");
