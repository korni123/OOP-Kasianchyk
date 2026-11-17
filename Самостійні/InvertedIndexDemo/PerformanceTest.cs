using System.Diagnostics;

namespace InvertedIndexDemo;

public class PerformanceTest
{
    private static readonly string[] Vocabulary =
    {
        "programming", "language", "modern", "popular", "simple", "fast", "object", "oriented",
        "syntax", "compiler", "library", "framework", "database", "network", "memory", "thread",
        "algorithm", "structure", "collection", "generic", "delegate", "event", "class", "interface",
        "windows", "linux", "cloud", "web", "mobile", "game", "data", "search", "index", "query"
    };

    public static List<Document> GenerateDocuments(int count)
    {
        Random random = new Random(42);
        List<Document> documents = new List<Document>();
        for (int i = 1; i <= count; i++)
        {
            List<string> words = new List<string>();
            for (int j = 0; j < 12; j++)
            {
                words.Add(Vocabulary[random.Next(Vocabulary.Length)]);
            }
            documents.Add(new Document
            {
                Id = i,
                Title = "Generated document " + i,
                Content = string.Join(" ", words) + "."
            });
        }
        return documents;
    }

    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();
        string normalizedWord = searchWord.ToLowerInvariant();
        List<Document> linearResults = documents.Where(d =>
            TextProcessor.Tokenize(d.Content).Contains(normalizedWord)).ToList();
        sw.Stop();
        long linearTicks = sw.ElapsedTicks;
        double linearMs = sw.Elapsed.TotalMilliseconds;

        InvertedIndex index = new InvertedIndex();
        foreach (Document doc in documents)
        {
            index.AddDocument(doc);
        }

        sw.Restart();
        List<Document> indexResults = index.SearchSingleWord(searchWord);
        sw.Stop();
        long indexTicks = sw.ElapsedTicks;
        double indexMs = sw.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Documents: {documents.Count}, word: '{searchWord}'");
        Console.WriteLine($"  Linear search: {linearTicks} ticks ({linearMs:F3} ms)");
        Console.WriteLine($"  Inverted Index: {indexTicks} ticks ({indexMs:F3} ms)");
        Console.WriteLine($"  Linear found: {linearResults.Count}, Index found: {indexResults.Count}");
    }
}
