// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public class ReadOnlyValueCollectionTests
{
    [Fact]
    public void Equal_returns_true_identical_value_collections()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));

        Assert.Equal(c1, c2);
    }

    [Fact]
    public void Equal_returns_false_nonidentical_value_collections()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ReadOnlyValueCollection<int>(Enumerable.Range(2, 20));

        Assert.NotEqual(c1, c2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var json = JsonSerializer.Serialize(c1);
        var c2 = JsonSerializer.Deserialize<ReadOnlyValueCollection<int>>(json);
        Assert.Equal(c1, c2);
    }
}
