using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    public class InvertedIndex
    {
        // Слово -> множина ID документів
        private Dictionary<string, HashSet<int>> _index;

        // ID -> документ (для швидкого отримання)
        private Dictionary<int, Document> _documents;

        public InvertedIndex()
        {
            _index = new Dictionary<string, HashSet<int>>();
            _documents = new Dictionary<int, Document>();
        }

        /// <summary>
        /// Додає документ до індексу
        /// </summary>
        public void AddDocument(Document doc)
        {
            _documents[doc.Id] = doc;

            foreach (string word in TextProcessor.Tokenize(doc.Content))
            {
                if (!_index.TryGetValue(word, out HashSet<int>? ids))
                {
                    ids = new HashSet<int>();
                    _index[word] = ids;
                }
                ids.Add(doc.Id);
            }
        }

        /// <summary>
        /// Шукає документи, що містять слово
        /// </summary>
        public List<Document> SearchSingleWord(string word)
        {
            string normalized = TextProcessor.NormalizeWord(word);
            if (_index.TryGetValue(normalized, out HashSet<int>? ids))
            {
                return ToDocuments(ids);
            }
            return new List<Document>();
        }

        /// <summary>
        /// Шукає документи, що містять ВСІ слова (AND)
        /// </summary>
        public List<Document> SearchMultipleWords(params string[] words)
        {
            List<string> normalized = NormalizeQuery(words);
            if (normalized.Count == 0)
            {
                return new List<Document>();
            }

            HashSet<int>? result = null;
            foreach (string word in normalized)
            {
                if (!_index.TryGetValue(word, out HashSet<int>? ids))
                {
                    return new List<Document>();
                }

                if (result == null)
                {
                    result = new HashSet<int>(ids);
                }
                else
                {
                    result.IntersectWith(ids);
                }
            }

            return ToDocuments(result!);
        }

        /// <summary>
        /// Шукає документи, що містять БУДЬ-ЯКЕ слово (OR)
        /// </summary>
        public List<Document> SearchAnyWord(params string[] words)
        {
            HashSet<int> result = new HashSet<int>();
            foreach (string word in NormalizeQuery(words))
            {
                if (_index.TryGetValue(word, out HashSet<int>? ids))
                {
                    result.UnionWith(ids);
                }
            }
            return ToDocuments(result);
        }

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;

        private static List<string> NormalizeQuery(string[] words)
        {
            List<string> tokens = words.SelectMany(TextProcessor.Tokenize).Distinct().ToList();
            List<string> withoutStopWords = TextProcessor.RemoveStopWords(tokens).ToList();
            return withoutStopWords.Count > 0 ? withoutStopWords : tokens;
        }

        private List<Document> ToDocuments(HashSet<int> ids)
        {
            return ids.OrderBy(id => id).Select(id => _documents[id]).ToList();
        }
    }
}
