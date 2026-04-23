// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class IntegerMathTests
{
    [Theory]
    [InlineData(11, 3, 4)]
    [InlineData(9, 3, 3)]
    [InlineData(1, 3, 1)]
    [InlineData(0, 3, 0)]
    [InlineData(10, 1, 10)]
    [InlineData(int.MaxValue, 1, int.MaxValue)]
    public void Int32_DivideByRoundUp_PositivePositive(int dividend, int divisor, int expected)
    {
        Assert.Equal(expected, dividend.DivideByRoundUp(divisor));
    }

    [Theory]
    [InlineData(-11, 3, -3)]
    [InlineData(-9, 3, -3)]
    [InlineData(11, -3, -3)]
    [InlineData(-11, -3, 4)]
    public void Int32_DivideByRoundUp_MixedSigns(int dividend, int divisor, int expected)
    {
        Assert.Equal(expected, dividend.DivideByRoundUp(divisor));
    }

    [Fact]
    public void Int32_DivideByRoundUp_DivisorZero_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => 10.DivideByRoundUp(0));
    }

    [Fact]
    public void Int32_DivideByRoundUp_MinValueDividedByMinusOne_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => int.MinValue.DivideByRoundUp(-1));
    }

    [Theory]
    [InlineData(11L, 3L, 4L)]
    [InlineData(9L, 3L, 3L)]
    [InlineData(0L, 5L, 0L)]
    [InlineData(long.MaxValue, 1L, long.MaxValue)]
    public void Int64_DivideByRoundUp_PositivePositive(long dividend, long divisor, long expected)
    {
        Assert.Equal(expected, dividend.DivideByRoundUp(divisor));
    }

    [Theory]
    [InlineData(-11L, 3L, -3L)]
    [InlineData(11L, -3L, -3L)]
    [InlineData(-11L, -3L, 4L)]
    public void Int64_DivideByRoundUp_MixedSigns(long dividend, long divisor, long expected)
    {
        Assert.Equal(expected, dividend.DivideByRoundUp(divisor));
    }

    [Fact]
    public void Int64_DivideByRoundUp_DivisorZero_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => 10L.DivideByRoundUp(0L));
    }

    [Fact]
    public void Int64_DivideByRoundUp_MinValueDividedByMinusOne_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => long.MinValue.DivideByRoundUp(-1L));
    }
}
