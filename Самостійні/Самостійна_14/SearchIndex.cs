namespace IndependentWork8;

public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex =
        new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);

    public int WordsCount => _invertedIndex.Count;

    private static string[] SplitWords(string text)
    {
        char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '"' };
        return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
    }

    public void AddDocument(int docId, string content)
    {
        foreach (string word in SplitWords(content))
        {
            if (!_invertedIndex.ContainsKey(word))
            {
                _invertedIndex[word] = new HashSet<int>();
            }
            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        if (_invertedIndex.TryGetValue(word, out HashSet<int>? docs))
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
