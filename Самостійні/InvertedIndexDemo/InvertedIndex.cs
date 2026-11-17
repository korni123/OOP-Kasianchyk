namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;

    public void AddDocument(Document doc)
    {
        _documents[doc.Id] = doc;
        foreach (string word in TextProcessor.Tokenize(doc.Content))
        {
            if (!_index.ContainsKey(word))
            {
                _index[word] = new HashSet<int>();
            }
            _index[word].Add(doc.Id);
        }
    }

    private HashSet<int> FindIds(string word)
    {
        HashSet<int> result = new HashSet<int>();
        foreach (string normalized in TextProcessor.Tokenize(word))
        {
            if (_index.TryGetValue(normalized, out HashSet<int>? ids))
            {
                result.UnionWith(ids);
            }
        }
        return result;
    }

    private List<Document> ToDocuments(HashSet<int> ids)
    {
        return ids.OrderBy(id => id).Select(id => _documents[id]).ToList();
    }

    public List<Document> SearchSingleWord(string word)
    {
        return ToDocuments(FindIds(word));
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        if (words.Length == 0)
        {
            return new List<Document>();
        }
        HashSet<int> result = FindIds(words[0]);
        for (int i = 1; i < words.Length; i++)
        {
            result.IntersectWith(FindIds(words[i]));
        }
        return ToDocuments(result);
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        HashSet<int> result = new HashSet<int>();
        foreach (string word in words)
        {
            result.UnionWith(FindIds(word));
        }
        return ToDocuments(result);
    }
}
