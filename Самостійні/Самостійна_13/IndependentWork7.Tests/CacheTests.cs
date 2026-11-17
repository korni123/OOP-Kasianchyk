using IndependentWork7;

namespace IndependentWork7.Tests;

public class CacheTests
{
    [Fact]
    public void Add_NewItem_IncreasesCount()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("a", 1);

        Assert.Equal(1, cache.Count);
        Assert.True(cache.ContainsKey("a"));
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueAndKeepsCount()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("a", 1);

        cache.Add("a", 100);

        Assert.Equal(1, cache.Count);
        Assert.Equal(100, cache.Get("a"));
    }

    [Fact]
    public void Get_ExistingKey_ReturnsValue()
    {
        var cache = new Cache<string, string>(3);
        cache.Add("a", "apple");
        cache.Add("b", "banana");

        var result = cache.Get("b");

        Assert.Equal("banana", result);
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("a", 1);

        Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
    }

    [Fact]
    public void Add_OverCapacity_RemovesOldestItem()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Add(3, "three");

        cache.Add(4, "four");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Add_OverCapacityTwice_RemovesItemsInFifoOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");

        cache.Add(3, "three");
        cache.Add(4, "four");

        Assert.False(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Get_DoesNotChangeEvictionOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Get(1);

        cache.Add(3, "three");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
    }

    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("a", 1);

        Assert.True(cache.ContainsKey("a"));
    }

    [Fact]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        var cache = new Cache<string, int>(3);

        Assert.False(cache.ContainsKey("a"));
    }

    [Fact]
    public void Count_EmptyCache_ReturnsZero()
    {
        var cache = new Cache<string, int>(3);

        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public void Count_NeverExceedsCapacity()
    {
        var cache = new Cache<int, int>(3);

        for (int i = 0; i < 10; i++)
        {
            cache.Add(i, i);
        }

        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Constructor_ZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<string, int>(0));
    }
}
