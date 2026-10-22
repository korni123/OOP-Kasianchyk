using System;
using System.Collections.Generic;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Constructor_ZeroCapacity_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Cache<int, string>(0));
        }

        [Fact]
        public void Add_NewItem_IncreasesCount()
        {
            var cache = new Cache<string, int>(3);

            cache.Add("a", 1);

            Assert.Equal(1, cache.Count);
        }

        [Fact]
        public void Add_ExistingKey_UpdatesValueWithoutGrowing()
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
            var cache = new Cache<int, string>(2);
            cache.Add(1, "one");
            cache.Add(2, "two");

            Assert.Equal("one", cache.Get(1));
            Assert.Equal("two", cache.Get(2));
        }

        [Fact]
        public void Get_MissingKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("x", "value");

            Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
        }

        [Fact]
        public void Add_OverCapacity_RemovesOldestItem()
        {
            var cache = new Cache<string, int>(3);
            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.Add("c", 3);

            cache.Add("d", 4);

            Assert.False(cache.ContainsKey("a"));
            Assert.True(cache.ContainsKey("b"));
            Assert.True(cache.ContainsKey("c"));
            Assert.True(cache.ContainsKey("d"));
            Assert.Equal(3, cache.Count);
        }

        [Fact]
        public void Add_OverCapacityTwice_RemovesItemsInFifoOrder()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("b", 2);

            cache.Add("c", 3);
            cache.Add("d", 4);

            Assert.False(cache.ContainsKey("a"));
            Assert.False(cache.ContainsKey("b"));
            Assert.Equal(3, cache.Get("c"));
            Assert.Equal(4, cache.Get("d"));
        }

        [Fact]
        public void Get_DoesNotChangeFifoOrder()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.Get("a");

            cache.Add("c", 3);

            Assert.False(cache.ContainsKey("a"));
            Assert.True(cache.ContainsKey("b"));
        }

        [Fact]
        public void Get_EvictedKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<int, int>(1);
            cache.Add(1, 10);
            cache.Add(2, 20);

            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void ContainsKey_ReturnsTrueForAddedAndFalseForMissing()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("key", "value");

            Assert.True(cache.ContainsKey("key"));
            Assert.False(cache.ContainsKey("other"));
        }

        [Fact]
        public void Count_EmptyCache_IsZero()
        {
            var cache = new Cache<string, string>(5);

            Assert.Equal(0, cache.Count);
        }

        [Fact]
        public void Count_NeverExceedsCapacity()
        {
            var cache = new Cache<int, int>(3);

            for (int i = 0; i < 10; i++)
            {
                cache.Add(i, i * i);
            }

            Assert.Equal(3, cache.Count);
            Assert.True(cache.ContainsKey(7));
            Assert.True(cache.ContainsKey(8));
            Assert.True(cache.ContainsKey(9));
        }
    }
}
