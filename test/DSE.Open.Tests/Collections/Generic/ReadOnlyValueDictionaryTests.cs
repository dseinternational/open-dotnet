// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Collections.Generic;

public class ReadOnlyValueDictionaryTests
{
    [Fact]
    public void Equal_returns_true_identical_value_dictionaries()
    {
        var d1 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var d2 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.True(d1.Equals(d2));
        Assert.True(d1 == d2);
    }

    [Fact]
    public void Equal_returns_true_reversed_value_dictionaries()
    {
        var d1 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())).Reverse());

        var d2 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.True(d1.Equals(d2));
        Assert.True(d1 == d2);
    }

    [Fact]
    public void Equal_returns_false_different_value_dictionaries()
    {
        var d1 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var d2 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(1, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        Assert.False(d1.Equals(d2));
        Assert.False(d1 == d2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var d1 = new ReadOnlyValueDictionary<int, string>(
            Enumerable.Range(0, 20).Select(i => new KeyValuePair<int, string>(i, i.ToStringInvariant())));

        var json = JsonSerializer.Serialize(d1);

        var d2 = JsonSerializer.Deserialize<ReadOnlyValueDictionary<int, string>>(json);

        Assert.Equal(d1, d2);
    }
}
