namespace lab7v9;

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new List<T>();

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Add(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (_items.Any(x => x.Id == item.Id))
        {
            throw new InvalidOperationException($"Item with Id {item.Id} already exists.");
        }
        _items.Add(item);
    }

    public void Update(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        int index = _items.FindIndex(x => x.Id == item.Id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Item with Id {item.Id} not found.");
        }
        _items[index] = item;
    }

    public void Delete(int id)
    {
        int index = _items.FindIndex(x => x.Id == id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Item with Id {id} not found.");
        }
        _items.RemoveAt(index);
    }
}
