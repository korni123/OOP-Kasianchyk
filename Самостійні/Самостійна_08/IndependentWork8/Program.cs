using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class FileMetadata
{
    public string FileName { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = "";

    public override string ToString()
    {
        return $"{FileName} ({Size} байт, змінено {LastModified:dd.MM.yyyy}, hash {Hash})";
    }
}

public class FileMetadataCache
{
    private Dictionary<string, FileMetadata> _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);

    public int Count
    {
        get { return _cache.Count; }
    }

    public void Add(FileMetadata metadata)
    {
        _cache[metadata.FileName] = metadata;
    }

    public FileMetadata? Get(string fileName)
    {
        if (_cache.TryGetValue(fileName, out FileMetadata? metadata))
        {
            return metadata;
        }
        return null;
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

public class TagManager
{
    private Dictionary<string, HashSet<string>> _articleTags = new Dictionary<string, HashSet<string>>();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
        bool added = _articleTags[articleId].Add(tag);
        if (!added)
        {
            Console.WriteLine($"  Тег '{tag}' вже є у статті {articleId}, дубль не додано");
        }
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            tags.Remove(tag);
        }
    }

    public HashSet<string> GetTags(string articleId)
    {
        if (_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            return new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase);
        }
        return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public HashSet<string> GetAllUniqueTags()
    {
        HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (HashSet<string> tags in _articleTags.Values)
        {
            result.UnionWith(tags);
        }
        return result;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);
        result.IntersectWith(GetTags(articleId2));
        return result;
    }

    public HashSet<string> FindUniqueTags(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);
        result.ExceptWith(GetTags(articleId2));
        return result;
    }
}

public class SearchIndex
{
    private Dictionary<string, HashSet<int>> _invertedIndex = new Dictionary<string, HashSet<int>>();

    public int WordCount
    {
        get { return _invertedIndex.Count; }
    }

    public void AddDocument(int docId, string content)
    {
        foreach (Match match in Regex.Matches(content.ToLowerInvariant(), @"\w+"))
        {
            string word = match.Value;
            if (!_invertedIndex.ContainsKey(word))
            {
                _invertedIndex[word] = new HashSet<int>();
            }
            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        if (_invertedIndex.TryGetValue(word.ToLowerInvariant(), out HashSet<int>? docs))
        {
            return new HashSet<int>(docs);
        }
        return new HashSet<int>();
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
        {
            return new HashSet<int>();
        }

        HashSet<int> result = Search(words[0]);
        for (int i = 1; i < words.Length; i++)
        {
            result.IntersectWith(Search(words[i]));
        }
        return result;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Сценарій 1: FileMetadataCache (Dictionary) ===");
        FileMetadataCache cache = new FileMetadataCache();
        cache.Add(new FileMetadata { FileName = "report.docx", Size = 48213, LastModified = new DateTime(2026, 10, 1), Hash = "a1f3" });
        cache.Add(new FileMetadata { FileName = "photo.png", Size = 1048576, LastModified = new DateTime(2026, 9, 14), Hash = "9bc0" });
        cache.Add(new FileMetadata { FileName = "notes.txt", Size = 512, LastModified = new DateTime(2026, 10, 20), Hash = "77de" });
        Console.WriteLine($"У кеші файлів: {cache.Count}");
        Console.WriteLine($"Get(\"REPORT.DOCX\") -> {cache.Get("REPORT.DOCX")}");
        Console.WriteLine($"Contains(\"photo.png\") = {cache.Contains("photo.png")}");
        Console.WriteLine($"Remove(\"photo.png\") = {cache.Remove("photo.png")}");
        Console.WriteLine($"Contains(\"photo.png\") = {cache.Contains("photo.png")}, у кеші файлів: {cache.Count}");
        Console.WriteLine($"Get(\"missing.pdf\") -> {(cache.Get("missing.pdf") == null ? "null (немає в кеші)" : "знайдено")}");

        Console.WriteLine();
        Console.WriteLine("=== Сценарій 2: TagManager (Dictionary + HashSet) ===");
        TagManager tags = new TagManager();
        tags.AddTag("art1", "csharp");
        tags.AddTag("art1", "dotnet");
        tags.AddTag("art1", "oop");
        tags.AddTag("art1", "CSharp");
        tags.AddTag("art2", "csharp");
        tags.AddTag("art2", "linq");
        tags.AddTag("art2", "oop");
        tags.AddTag("art3", "python");
        tags.AddTag("art3", "ai");
        Console.WriteLine($"Теги art1: {string.Join(", ", tags.GetTags("art1"))}");
        Console.WriteLine($"Теги art2: {string.Join(", ", tags.GetTags("art2"))}");
        Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tags.GetAllUniqueTags())}");
        Console.WriteLine($"Спільні теги art1 і art2 (IntersectWith): {string.Join(", ", tags.FindCommonTags("art1", "art2"))}");
        Console.WriteLine($"Є в art1, але немає в art2 (ExceptWith): {string.Join(", ", tags.FindUniqueTags("art1", "art2"))}");
        tags.RemoveTag("art1", "dotnet");
        Console.WriteLine($"Після RemoveTag(art1, dotnet): {string.Join(", ", tags.GetTags("art1"))}");

        Console.WriteLine();
        Console.WriteLine("=== Сценарій 3: SearchIndex (Dictionary<string, HashSet<int>>) ===");
        SearchIndex index = new SearchIndex();
        index.AddDocument(1, "C# is a modern object-oriented programming language.");
        index.AddDocument(2, "Java is a popular programming language.");
        index.AddDocument(3, "Python is simple and popular.");
        index.AddDocument(4, "C# and Java are both object-oriented.");
        Console.WriteLine($"Слів в індексі: {index.WordCount}");
        Console.WriteLine($"Search(\"popular\") -> документи [{string.Join(", ", index.Search("popular"))}]");
        Console.WriteLine($"Search(\"programming\") -> документи [{string.Join(", ", index.Search("programming"))}]");
        Console.WriteLine($"SearchMultiple(\"object\", \"java\") -> документи [{string.Join(", ", index.SearchMultiple("object", "java"))}]");
        Console.WriteLine($"SearchMultiple(\"popular\", \"language\") -> документи [{string.Join(", ", index.SearchMultiple("popular", "language"))}]");
        Console.WriteLine($"Search(\"rust\") -> документи [{string.Join(", ", index.Search("rust"))}]");

        Console.WriteLine();
        Console.WriteLine("=== Custom comparer: HashSet<Person> з PersonNameComparer ===");
        HashSet<Person> people = new HashSet<Person>(new PersonNameComparer());
        Console.WriteLine($"Add(John, 25) = {people.Add(new Person { Name = "John", Age = 25 })}");
        Console.WriteLine($"Add(John, 30) = {people.Add(new Person { Name = "John", Age = 30 })}");
        Console.WriteLine($"Add(Anna, 22) = {people.Add(new Person { Name = "Anna", Age = 22 })}");
        Console.WriteLine($"У множині людей: {people.Count}");
    }
}

public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

public class PersonNameComparer : IEqualityComparer<Person>
{
    public bool Equals(Person? x, Person? y)
    {
        return x?.Name == y?.Name;
    }

    public int GetHashCode(Person obj)
    {
        return obj.Name.GetHashCode();
    }
}
