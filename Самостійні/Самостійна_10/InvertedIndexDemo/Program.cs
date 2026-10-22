using System;
using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Частина 3: Вимірювання продуктивності ===");
            PerformanceTest.Warmup(GenerateDocuments(100));
            PerformanceTest.PrintHeader();
            foreach (int count in new[] { 10, 100, 1000, 10000 })
            {
                List<Document> docs = GenerateDocuments(count);
                PerformanceTest.CompareSearchMethods(docs, "programming");
                PerformanceTest.CompareSearchMethods(docs, "microsoft");
            }

            Console.WriteLine();
            Console.WriteLine("=== Частина 4: Інтерактивна демонстрація ===");
            var index = new InvertedIndex();
            LoadDocuments(index);

            Console.WriteLine($"Loaded {index.DocumentCount} documents");
            Console.WriteLine($"Index contains {index.UniqueWords} unique words");
            Console.WriteLine();

            Console.WriteLine("Демо OR-пошуку: SearchAnyWord(\"python\", \"linq\")");
            foreach (var doc in index.SearchAnyWord("python", "linq"))
            {
                Console.WriteLine($"  {doc}");
            }
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter search query (or 'exit'): ");
                var query = Console.ReadLine();

                if (query == null || query.Trim() == "exit") break;

                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("\n--- Search Results (AND) ---");
                var results = index.SearchMultipleWords(words);
                foreach (var doc in results)
                {
                    Console.WriteLine(doc);
                    Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
                }

                Console.WriteLine($"\nFound {results.Count} documents\n");
            }
        }

        static List<Document> GetBaseDocuments()
        {
            return new List<Document>
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
        }

        static void LoadDocuments(InvertedIndex index)
        {
            foreach (var doc in GetBaseDocuments())
            {
                index.AddDocument(doc);
            }
        }

        static List<Document> GenerateDocuments(int count)
        {
            string[] dictionary =
            {
                "data", "cloud", "server", "database", "network", "algorithm", "compiler", "testing",
                "framework", "library", "interface", "class", "object", "memory", "thread", "cache",
                "security", "api", "frontend", "backend", "mobile", "game", "engine", "query"
            };

            var baseDocs = GetBaseDocuments();
            var random = new Random(42);
            var result = new List<Document>();

            for (int i = 1; i <= count; i++)
            {
                var source = baseDocs[(i - 1) % baseDocs.Count];
                int extraWords = random.Next(5, 16);
                var extra = new List<string>();
                for (int j = 0; j < extraWords; j++)
                {
                    extra.Add(dictionary[random.Next(dictionary.Length)]);
                }

                result.Add(new Document
                {
                    Id = i,
                    Title = $"{source.Title} #{i}",
                    Content = source.Content + " " + string.Join(" ", extra)
                });
            }

            return result;
        }
    }
}
