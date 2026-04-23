// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class DoubleExtensionsTests
{
    [Theory]
    [InlineData(0.0, 0)]
    [InlineData(1.0, 0)]
    [InlineData(-1.0, 0)]
    [InlineData(1.5, 1)]
    [InlineData(-1.25, 2)]
    [InlineData(0.125, 3)]
    public void GetDecimalPlacesCount(double value, int expected)
    {
        Assert.Equal(expected, value.GetDecimalPlacesCount());
    }

    [Theory]
    [InlineData(0.0, false)]
    [InlineData(1.0, false)]
    [InlineData(-1.0, false)]
    [InlineData(1.5, true)]
    [InlineData(0.125, true)]
    public void HasDecimalPlaces(double value, bool expected)
    {
        Assert.Equal(expected, value.HasDecimalPlaces());
    }

    [Fact]
    public void GetRepeatableHashCode_EqualValuesProduceEqualHashes()
    {
        Assert.Equal(3.14.GetRepeatableHashCode(), 3.14.GetRepeatableHashCode());
    }
}
