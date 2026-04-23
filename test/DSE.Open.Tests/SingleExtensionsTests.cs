// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class SingleExtensionsTests
{
    [Theory]
    [InlineData(0f, 0)]
    [InlineData(1f, 0)]
    [InlineData(-1f, 0)]
    [InlineData(1.5f, 1)]
    [InlineData(-1.25f, 2)]
    [InlineData(0.125f, 3)]
    public void GetDecimalPlacesCount(float value, int expected)
    {
        Assert.Equal(expected, value.GetDecimalPlacesCount());
    }

    [Theory]
    [InlineData(0f, false)]
    [InlineData(1f, false)]
    [InlineData(1.5f, true)]
    [InlineData(0.125f, true)]
    public void HasDecimalPlaces(float value, bool expected)
    {
        Assert.Equal(expected, value.HasDecimalPlaces());
    }

    [Fact]
    public void GetRepeatableHashCode_EqualValuesProduceEqualHashes()
    {
        Assert.Equal(3.14f.GetRepeatableHashCode(), 3.14f.GetRepeatableHashCode());
    }
}
