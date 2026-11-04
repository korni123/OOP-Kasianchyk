namespace lab8v9;

public static class CollectionHelper<T>
{
    public static T First(IEnumerable<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }
        foreach (T item in collection)
        {
            if (predicate(item))
            {
                return item;
            }
        }
        throw new InvalidOperationException("No element satisfies the condition.");
    }
}
