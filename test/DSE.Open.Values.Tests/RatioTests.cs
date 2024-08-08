// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values.Tests;

public sealed class RatioTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (Ratio)0.57852m;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }
}
