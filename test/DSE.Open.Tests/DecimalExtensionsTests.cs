// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class DecimalExtensionsTests
{
    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 0)]
    [InlineData("-1", 0)]
    [InlineData("1.5", 1)]
    [InlineData("-1.5", 1)]
    [InlineData("1.25", 2)]
    [InlineData("0.001", 3)]
    [InlineData("123.456", 3)]
    public void GetDecimalPlacesCount(string value, int expected)
    {
        var d = decimal.Parse(value, CultureInfo.InvariantCulture);

        Assert.Equal(expected, d.GetDecimalPlacesCount());
    }

    [Theory]
    [InlineData("0", false)]
    [InlineData("1", false)]
    [InlineData("-1", false)]
    [InlineData("1.5", true)]
    [InlineData("0.001", true)]
    public void HasDecimalPlaces(string value, bool expected)
    {
        var d = decimal.Parse(value, CultureInfo.InvariantCulture);

        Assert.Equal(expected, d.HasDecimalPlaces());
    }

    [Fact]
    public void GetRepeatableHashCode_EqualValuesProduceEqualHashes()
    {
        var a = 123.456m;
        var b = 123.456m;

        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_DifferentValuesUsuallyDiffer()
    {
        Assert.NotEqual(1m.GetRepeatableHashCode(), 2m.GetRepeatableHashCode());
    }
}
