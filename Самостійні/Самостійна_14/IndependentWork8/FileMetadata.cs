namespace IndependentWork8;

public class FileMetadata
{
    public string FileName { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; }

    public FileMetadata(string fileName, long size, DateTime lastModified, string hash)
    {
        FileName = fileName;
        Size = size;
        LastModified = lastModified;
        Hash = hash;
    }

    public override string ToString()
    {
        return $"{FileName}, {Size} bytes, {LastModified:dd.MM.yyyy HH:mm}, hash {Hash}";
    }
}
