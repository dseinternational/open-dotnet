// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class PercentTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (Percent)37.57852m;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Theory]
    [InlineData(50, 0.5)]
    [InlineData(100, 1.0)]
    [InlineData(-100, -1.0)]
    [InlineData(0, 0)]
    [InlineData(25, 0.25)]
    public void ToRatio_ReturnsCorrectValue(decimal percentValue, decimal expectedRatio)
    {
        var percent = (Percent)percentValue;

        var ratio = percent.ToRatio();

        Assert.Equal(expectedRatio, (decimal)ratio);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(-100)]
    [InlineData(0)]
    public void FromRatio_RoundtripsWithToRatio(decimal percentValue)
    {
        var percent = (Percent)percentValue;

        var ratio = percent.ToRatio();
        var roundtripped = Percent.FromRatio(ratio);

        Assert.Equal(percent, roundtripped);
    }
}
