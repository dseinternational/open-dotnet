// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Collections.Generic;

public class ReadOnlySortedValueDictionaryTests
{
    [Fact]
    public void Init_sorts_by_key()
    {
        var d1 = new ReadOnlySortedValueDictionary<int, string>(
            [
                new(3, "3"),
                new(1, "1"),
                new(2, "2"),
            ]);

        var keys = d1.Keys.ToArray();

        Assert.Equal(1, keys[0]);
        Assert.Equal(2, keys[1]);
        Assert.Equal(3, keys[2]);
    }

    [Fact]
    public void Equal_returns_true_identical_value_dictionaries()
    {
        var d1 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var d2 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.True(d1.Equals(d2));
        Assert.True(d1 == d2);
    }

    [Fact]
    public void Equal_returns_true_reversed_value_dictionaries()
    {
        var d1 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())).Reverse());

        var d2 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.True(d1.Equals(d2));
        Assert.True(d1 == d2);
    }

    [Fact]
    public void Equal_returns_false_different_value_dictionaries()
    {
        var d1 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var d2 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(1, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.False(d1.Equals(d2));
        Assert.False(d1 == d2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var d1 = new ReadOnlySortedValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var json = JsonSerializer.Serialize(d1);

        var d2 = JsonSerializer.Deserialize<ReadOnlySortedValueDictionary<int, string>>(json);

        Assert.Equal(d1, d2);
    }
}
