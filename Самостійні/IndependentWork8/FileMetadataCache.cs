namespace IndependentWork8;

public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache =
        new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);

    public int Count => _cache.Count;

    public bool Add(FileMetadata metadata)
    {
        return _cache.TryAdd(metadata.FileName, metadata);
    }

    public FileMetadata? Get(string fileName)
    {
        _cache.TryGetValue(fileName, out FileMetadata? metadata);
        return metadata;
    }

    public bool Remove(string fileName)
    {
        return _cache.Remove(fileName);
    }

    public bool Contains(string fileName)
    {
        return _cache.ContainsKey(fileName);
    }
}
