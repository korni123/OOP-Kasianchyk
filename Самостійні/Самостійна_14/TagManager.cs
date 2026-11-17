namespace IndependentWork8;

public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags =
        new Dictionary<string, HashSet<string>>();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
        _articleTags[articleId].Add(tag);
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
