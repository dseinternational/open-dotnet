// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Collections.Generic;

public class ValueCollectionTests
{
    [Fact]
    public void Equal()
    {
        var c1 = new ValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ValueCollection<int>(Enumerable.Range(0, 20));

        Assert.True(c1.Equals(c2));
        Assert.True(c1 == c2);
        Assert.False(c1 != c2);
    }

    [Fact]
    public void Not_equal()
    {
        var c1 = new ValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ValueCollection<int>(Enumerable.Range(2, 20));

        Assert.False(c1.Equals(c2));
        Assert.False(c1 == c2);
        Assert.True(c1 != c2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var c1 = new ValueCollection<int>(Enumerable.Range(0, 20));
        var json = JsonSerializer.Serialize(c1);
        var c2 = JsonSerializer.Deserialize<ValueCollection<int>>(json);
        Assert.Equal(c1, c2);
    }
}
