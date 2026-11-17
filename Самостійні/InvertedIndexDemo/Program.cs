using InvertedIndexDemo;

List<Document> documents = new List<Document>
{
    new Document
    {
        Id = 1,
        Title = "Introduction to C#",
        Content = "C# is a modern object-oriented programming language developed by Microsoft."
    },
    new Document
    {
        Id = 2,
        Title = "Java Basics",
        Content = "Java is a popular programming language known for its portability and performance."
    },
    new Document
    {
        Id = 3,
        Title = "Python Overview",
        Content = "Python is a high-level programming language with simple syntax."
    },
    new Document
    {
        Id = 4,
        Title = "C# Advanced Features",
        Content = "C# supports LINQ, async/await, and many modern programming paradigms."
    },
    new Document
    {
        Id = 5,
        Title = "Comparing Languages",
        Content = "C#, Java, and Python are among the most popular programming languages today."
    }
};

InvertedIndex index = new InvertedIndex();
foreach (Document doc in documents)
{
    index.AddDocument(doc);
}

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words");
Console.WriteLine();

void ShowResults(string title, List<Document> results)
{
    Console.WriteLine(title);
    foreach (Document doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine("  " + doc.Content.Substring(0, Math.Min(100, doc.Content.Length)) + "...");
    }
    Console.WriteLine($"Found {results.Count} documents");
    Console.WriteLine();
}

ShowResults("Query: programming language --- Search Results (AND) ---",
    index.SearchMultipleWords("programming", "language"));
ShowResults("Query: c# async --- Search Results (AND) ---",
    index.SearchMultipleWords("c#", "async"));
ShowResults("Query: python java --- Search Results (OR) ---",
    index.SearchAnyWord("python", "java"));
ShowResults("Query: portability --- Single word ---",
    index.SearchSingleWord("portability"));

Console.WriteLine("Stop words removed from a sentence:");
IEnumerable<string> tokens = TextProcessor.Tokenize("The cat is on the table, and the dog is in the garden!");
Console.WriteLine("  Tokens: " + string.Join(", ", tokens));
Console.WriteLine("  Without stop words: " + string.Join(", ", TextProcessor.RemoveStopWords(tokens)));
Console.WriteLine();

Console.WriteLine("=== Performance: linear search vs inverted index ===");
PerformanceTest.CompareSearchMethods(documents, "programming");
PerformanceTest.CompareSearchMethods(PerformanceTest.GenerateDocuments(10), "programming");
PerformanceTest.CompareSearchMethods(PerformanceTest.GenerateDocuments(100), "programming");
PerformanceTest.CompareSearchMethods(PerformanceTest.GenerateDocuments(1000), "programming");
PerformanceTest.CompareSearchMethods(PerformanceTest.GenerateDocuments(10000), "programming");
