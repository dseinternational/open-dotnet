// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class RatioTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (Ratio)0.57852m;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 50)]
    [InlineData(1, 100)]
    [InlineData(-1, -100)]
    public void ToPercent_MultipliesByOneHundred(double ratio, double expectedPercent)
    {
        var value = (Ratio)(decimal)ratio;
        var percent = value.ToPercent();
        Assert.Equal((decimal)expectedPercent, (decimal)percent);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(50, 0.5)]
    [InlineData(100, 1)]
    [InlineData(-100, -1)]
    public void FromPercent_DividesByOneHundred(double percent, double expectedRatio)
    {
        var value = (Percent)(decimal)percent;
        var ratio = Ratio.FromPercent(value);
        Assert.Equal((decimal)expectedRatio, (decimal)ratio);
    }
}
