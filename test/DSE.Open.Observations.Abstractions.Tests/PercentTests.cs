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
    [InlineData(0, 0)]
    [InlineData(50, 0.5)]
    [InlineData(100, 1)]
    [InlineData(-100, -1)]
    [InlineData(25, 0.25)]
    public void ToRatio_DividesByOneHundred(double percent, double expectedRatio)
    {
        var value = (Percent)(decimal)percent;
        var ratio = value.ToRatio();
        Assert.Equal((decimal)expectedRatio, (decimal)ratio);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 50)]
    [InlineData(1, 100)]
    [InlineData(-1, -100)]
    [InlineData(0.25, 25)]
    public void FromRatio_MultipliesByOneHundred(double ratio, double expectedPercent)
    {
        var value = (Ratio)(decimal)ratio;
        var percent = Percent.FromRatio(value);
        Assert.Equal((decimal)expectedPercent, (decimal)percent);
    }

    [Fact]
    public void ToRatio_FromRatio_Roundtrip()
    {
        var original = (Ratio)0.37m;
        var roundtripped = Percent.FromRatio(original).ToRatio();
        Assert.Equal(original, roundtripped);
    }
}
