using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo
{
    public class PerformanceTest
    {
        private const int LinearRepeats = 20;
        private const int IndexRepeats = 10000;

        /// <summary>
        /// Порівнює лінійний пошук і пошук через InvertedIndex на одному наборі документів.
        /// Обидва підходи використовують однакову нормалізацію (TextProcessor.Tokenize).
        /// </summary>
        public static void CompareSearchMethods(List<Document> documents, string searchWord)
        {
            var sw = new Stopwatch();
            string normalizedWord = TextProcessor.NormalizeWord(searchWord);

            List<Document> linearResults = new List<Document>();
            sw.Start();
            for (int i = 0; i < LinearRepeats; i++)
            {
                linearResults = documents.Where(d =>
                    TextProcessor.Tokenize(d.Content).Contains(normalizedWord)).ToList();
            }
            sw.Stop();
            double linearUs = sw.Elapsed.TotalMicroseconds / LinearRepeats;
            long linearTicks = sw.ElapsedTicks / LinearRepeats;

            sw.Restart();
            var index = new InvertedIndex();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }
            sw.Stop();
            double buildUs = sw.Elapsed.TotalMicroseconds;

            List<Document> indexResults = new List<Document>();
            sw.Restart();
            for (int i = 0; i < IndexRepeats; i++)
            {
                indexResults = index.SearchSingleWord(searchWord);
            }
            sw.Stop();
            double indexUs = sw.Elapsed.TotalMicroseconds / IndexRepeats;
            long indexTicks = sw.ElapsedTicks / IndexRepeats;

            string check = linearResults.Count == indexResults.Count ? "OK" : "РІЗНИЦЯ!";
            Console.WriteLine(
                $"{documents.Count,7} | {searchWord,-12} | {linearUs,13:F1} | {linearTicks,10} | {indexUs,11:F2} | {indexTicks,10} | {buildUs / 1000,12:F2} | {linearResults.Count,6} / {indexResults.Count,-6} {check}");
        }

        public static void PrintHeader()
        {
            Console.WriteLine($"Stopwatch.Frequency = {Stopwatch.Frequency} тіків/с; лінійний пошук усереднено по {LinearRepeats} запусках, індекс - по {IndexRepeats}");
            Console.WriteLine($"{"Докум.",7} | {"Слово",-12} | {"Лінійний, мкс",13} | {"Лін. тіки",10} | {"Індекс, мкс",11} | {"Інд. тіки",10} | {"Побудова, мс",12} | Знайдено (лін/інд)");
            Console.WriteLine(new string('-', 118));
        }

        public static void Warmup(List<Document> documents)
        {
            var index = new InvertedIndex();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }
            index.SearchSingleWord("c#");
            documents.Where(d => TextProcessor.Tokenize(d.Content).Contains("c#")).ToList();
        }
    }
}
