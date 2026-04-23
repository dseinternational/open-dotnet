// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class DictionaryEqualityComparerTests
{
    private static readonly DictionaryEqualityComparer<string, int> s_comparer =
        DictionaryEqualityComparer<string, int>.Default;

    [Fact]
    public void Equals_BothNull_ReturnsTrue()
    {
        Assert.True(s_comparer.Equals((IDictionary<string, int>?)null, null));
        Assert.True(s_comparer.Equals((IReadOnlyDictionary<string, int>?)null, null));
    }

    [Fact]
    public void Equals_OneNull_ReturnsFalse()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };

        Assert.False(s_comparer.Equals(dict, null));
        Assert.False(s_comparer.Equals(null, dict));
    }

    [Fact]
    public void Equals_SameEntries_ReturnsTrue()
    {
        IDictionary<string, int> a = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        IDictionary<string, int> b = new Dictionary<string, int> { ["b"] = 2, ["a"] = 1 };

        Assert.True(s_comparer.Equals(a, b));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        IDictionary<string, int> a = new Dictionary<string, int> { ["a"] = 1 };
        IDictionary<string, int> b = new Dictionary<string, int> { ["a"] = 2 };

        Assert.False(s_comparer.Equals(a, b));
    }

    [Fact]
    public void Equals_DifferentKeys_ReturnsFalse()
    {
        IDictionary<string, int> a = new Dictionary<string, int> { ["a"] = 1 };
        IDictionary<string, int> b = new Dictionary<string, int> { ["b"] = 1 };

        Assert.False(s_comparer.Equals(a, b));
    }

    [Fact]
    public void Equals_DifferentCounts_ReturnsFalse()
    {
        IDictionary<string, int> a = new Dictionary<string, int> { ["a"] = 1 };
        IDictionary<string, int> b = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Assert.False(s_comparer.Equals(a, b));
    }

    [Fact]
    public void GetHashCode_SameInstance_ReturnsSameHash()
    {
        IDictionary<string, int> a = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Assert.Equal(s_comparer.GetHashCode(a), s_comparer.GetHashCode(a));
    }

    [Fact]
    public void GetHashCode_NullThrows()
    {
        _ = Assert.Throws<ArgumentNullException>(() => s_comparer.GetHashCode((IDictionary<string, int>)null!));
        _ = Assert.Throws<ArgumentNullException>(() => s_comparer.GetHashCode((IReadOnlyDictionary<string, int>)null!));
    }

    [Fact]
    public void Default_ReturnsSingleton()
    {
        var a = DictionaryEqualityComparer<string, int>.Default;
        var b = DictionaryEqualityComparer<string, int>.Default;

        Assert.Same(a, b);
    }
}
