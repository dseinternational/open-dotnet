// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open;

public class DiscriminatedTests
{
    [Fact]
    public void IsEqualIfValuesAndDiscriminatorsEqual()
    {
        var d0 = Discriminated.Create(42, "a");
        var d1 = Discriminated.Create(42, "a");
        Assert.Equal(d0, d1);
    }

    [Fact]
    public void NotEqualIfValuesNotEqual()
    {
        var d0 = Discriminated.Create(42, "a");
        var d1 = Discriminated.Create(41, "a");
        Assert.NotEqual(d0, d1);
    }

    [Fact]
    public void NotEqualIfDiscriminatorsNotEqual()
    {
        var d0 = Discriminated.Create(42, "a");
        var d1 = Discriminated.Create(42, "b");
        Assert.NotEqual(d0, d1);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var d0 = Discriminated.Create(42, "a");
        var json = JsonSerializer.Serialize(d0);
        var d1 = JsonSerializer.Deserialize<Discriminated<int, string>>(json);
        Assert.Equal(d0, d1);
    }
}
