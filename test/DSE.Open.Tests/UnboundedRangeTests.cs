// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class UnboundedRangeTests
{
    [Fact]
    public void Constructor_BothBounds_StoresValues()
    {
        var range = new UnboundedRange<int>(1, 10);

        Assert.Equal(1, range.Start);
        Assert.Equal(10, range.End);
    }

    [Fact]
    public void Constructor_StartGreaterThanEnd_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new UnboundedRange<int>(10, 1));
    }

    [Fact]
    public void Constructor_EqualBounds_Allowed()
    {
        var range = new UnboundedRange<int>(5, 5);

        Assert.Equal(5, range.Start);
        Assert.Equal(5, range.End);
    }

    [Fact]
    public void Constructor_NullBounds_StoresNull()
    {
        var range = new UnboundedRange<int>(null, null);

        Assert.Null(range.Start);
        Assert.Null(range.End);
    }

    [Fact]
    public void Infinite_BothBoundsNull()
    {
        var range = UnboundedRange<int>.Infinite;

        Assert.Null(range.Start);
        Assert.Null(range.End);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    public void Includes_Bounded(int value, bool expected)
    {
        var range = new UnboundedRange<int>(1, 10);

        Assert.Equal(expected, range.Includes(value));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(5, true)]
    [InlineData(int.MaxValue, true)]
    public void Includes_GreaterThanOrEqual(int value, bool expected)
    {
        var range = UnboundedRange.GreaterThanOrEqual(5);

        Assert.Equal(expected, range.Includes(value));
    }

    [Theory]
    [InlineData(int.MinValue, true)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    public void Includes_LessThanOrEqual(int value, bool expected)
    {
        var range = UnboundedRange.LessThanOrEqual(5);

        Assert.Equal(expected, range.Includes(value));
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    public void Includes_Infinite_ReturnsTrueForAnyValue(int value)
    {
        Assert.True(UnboundedRange<int>.Infinite.Includes(value));
    }

    [Fact]
    public void Between_StoresBothBounds()
    {
        var range = UnboundedRange.Between(2, 8);

        Assert.Equal(2, range.Start);
        Assert.Equal(8, range.End);
    }

    [Fact]
    public void GreaterThanOrEqual_HasNoUpperBound()
    {
        var range = UnboundedRange.GreaterThanOrEqual(2);

        Assert.Equal(2, range.Start);
        Assert.Null(range.End);
    }

    [Fact]
    public void LessThanOrEqual_HasNoLowerBound()
    {
        var range = UnboundedRange.LessThanOrEqual(2);

        Assert.Null(range.Start);
        Assert.Equal(2, range.End);
    }

    [Fact]
    public void Equality_SameBoundsAreEqual()
    {
        var a = new UnboundedRange<int>(1, 10);
        var b = new UnboundedRange<int>(1, 10);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentBoundsAreNotEqual()
    {
        var a = new UnboundedRange<int>(1, 10);
        var b = new UnboundedRange<int>(1, 11);

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }
}
