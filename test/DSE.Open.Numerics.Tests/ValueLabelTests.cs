// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics;

public class ValueLabelTests
{
    [Fact]
    public void ValueLabel_Equality()
    {
        var a = new ValueLabel<int>(1, "a");
        var b = new ValueLabel<int>(1, "b");
        var c = new ValueLabel<int>(2, "a");
        var a2 = new ValueLabel<int>(1, "a");
        Assert.NotEqual(a, b);
        Assert.NotEqual(a, c);
        Assert.NotEqual(b, c);
        Assert.Equal(a, a2);
    }

    [Fact]
    public void Serialize()
    {
        var a = new ValueLabel<int>(1, "a");
        var json = JsonSerializer.Serialize(a);
        Assert.Equal("{\"value\":1,\"label\":\"a\"}", json);
    }

    [Fact]
    public void Deserialize()
    {
        var json = "{\"value\":1,\"label\":\"a\"}";
        var a = JsonSerializer.Deserialize<ValueLabel<int>>(json);
        Assert.Equal(1, a.Value);
        Assert.Equal("a", a.Label);
    }
}
