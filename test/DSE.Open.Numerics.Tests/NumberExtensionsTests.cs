// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NumberExtensionsTests
{
    [Fact]
    public void ToInt32Checked_WithOverflow_ShouldThrowOverflowException()
    {
        _ = Assert.Throws<OverflowException>(() => ((long)int.MaxValue + 1).ToInt32Checked());
    }

    [Theory]
    [InlineData(long.MinValue, int.MinValue)]
    [InlineData((long)int.MaxValue + 1, int.MaxValue)]
    public void ToInt32Saturating_WithOutOfRangeValue_ShouldClamp(long value, int expected)
    {
        Assert.Equal(expected, value.ToInt32Saturating());
    }

    [Fact]
    public void ToInt32Truncating_WithFractionalValue_ShouldTruncate()
    {
        Assert.Equal(42, 42.9.ToInt32Truncating());
    }

    [Fact]
    public void ToUInt32Checked_WithNegativeValue_ShouldThrowOverflowException()
    {
        _ = Assert.Throws<OverflowException>(() => (-1).ToUInt32Checked());
    }

    [Theory]
    [InlineData(-1L, 0U)]
    [InlineData((long)uint.MaxValue + 1, uint.MaxValue)]
    public void ToUInt32Saturating_WithOutOfRangeValue_ShouldClamp(long value, uint expected)
    {
        Assert.Equal(expected, value.ToUInt32Saturating());
    }

    [Fact]
    public void ToUInt32Truncating_WithNegativeValue_ShouldWrap()
    {
        Assert.Equal(uint.MaxValue, (-1).ToUInt32Truncating());
    }

    [Fact]
    public void ToInt64Checked_WithOverflow_ShouldThrowOverflowException()
    {
        _ = Assert.Throws<OverflowException>(() => double.PositiveInfinity.ToInt64Checked());
    }

    [Theory]
    [InlineData(double.NegativeInfinity, long.MinValue)]
    [InlineData(double.PositiveInfinity, long.MaxValue)]
    public void ToInt64Saturating_WithOutOfRangeValue_ShouldClamp(double value, long expected)
    {
        Assert.Equal(expected, value.ToInt64Saturating());
    }

    [Fact]
    public void ToInt64Truncating_WithFractionalValue_ShouldTruncate()
    {
        Assert.Equal(42L, 42.9.ToInt64Truncating());
    }

    [Fact]
    public void ToUInt64Checked_WithNegativeValue_ShouldThrowOverflowException()
    {
        _ = Assert.Throws<OverflowException>(() => (-1).ToUInt64Checked());
    }

    [Theory]
    [InlineData(-1.0, 0UL)]
    [InlineData(double.PositiveInfinity, ulong.MaxValue)]
    public void ToUInt64Saturating_WithOutOfRangeValue_ShouldClamp(double value, ulong expected)
    {
        Assert.Equal(expected, value.ToUInt64Saturating());
    }

    [Fact]
    public void ToUInt64Truncating_WithFractionalValue_ShouldTruncate()
    {
        Assert.Equal(42UL, 42.9.ToUInt64Truncating());
    }
}
