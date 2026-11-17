using System.Text.RegularExpressions;

namespace IndependentWork16;

public static class TextProcessor
{
    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    public static IEnumerable<string> Tokenize(string text)
    {
        foreach (Match match in Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\p{N}#]+"))
        {
            yield return match.Value;
        }
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        return words.Where(w => !StopWords.Contains(w));
    }
}
