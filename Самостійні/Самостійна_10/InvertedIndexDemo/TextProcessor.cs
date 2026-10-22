using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    public static class TextProcessor
    {
        private static readonly Regex WordRegex = new Regex(@"[\w#+]+", RegexOptions.Compiled);

        private static readonly HashSet<string> StopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        /// <summary>
        /// Розбиває текст на слова, видаляє пунктуацію, переводить у нижній регістр.
        /// "C#, Java!" -> ["c#", "java"]
        /// </summary>
        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Enumerable.Empty<string>();
            }

            return WordRegex.Matches(text.ToLowerInvariant()).Select(m => m.Value);
        }

        /// <summary>
        /// Нормалізує одне слово запиту так само, як текст документа.
        /// </summary>
        public static string NormalizeWord(string word)
        {
            return Tokenize(word).FirstOrDefault() ?? "";
        }

        /// <summary>
        /// Видаляє стоп-слова (a, an, the, is, are, etc.)
        /// </summary>
        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            return words.Where(w => !StopWords.Contains(w));
        }
    }
}
