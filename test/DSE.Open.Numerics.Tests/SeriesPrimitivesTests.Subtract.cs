// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void Subtract_SeriesSeries_ReturnsDifference()
    {
        var lhs = Series.Create([10, 20, 30, 40, 50], "a");
        var rhs = Series.Create([1, 2, 3, 4, 5], "b");

        var result = lhs.Subtract(rhs);

        Assert.Equal(9, result[0]);
        Assert.Equal(18, result[1]);
        Assert.Equal(27, result[2]);
        Assert.Equal(36, result[3]);
        Assert.Equal(45, result[4]);
    }

    [Fact]
    public void Subtract_SeriesSeries_IntoSpan_WritesDifference()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([1, 2, 3]);
        Span<int> destination = stackalloc int[3];

        lhs.Subtract(rhs, destination);

        Assert.Equal(9, destination[0]);
        Assert.Equal(18, destination[1]);
        Assert.Equal(27, destination[2]);
    }

    [Fact]
    public void Subtract_SeriesSeries_IntoAliasedRightHandSpan_WritesDifference()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([1, 2, 3]);

        lhs.Subtract(rhs, rhs.AsSpan());

        Assert.Equal(9, rhs[0]);
        Assert.Equal(18, rhs[1]);
        Assert.Equal(27, rhs[2]);
    }

    [Fact]
    public void Subtract_SeriesScalar_ReturnsDifference()
    {
        var series = Series.Create([10, 20, 30]);

        var result = series.Subtract(5);

        Assert.Equal(5, result[0]);
        Assert.Equal(15, result[1]);
        Assert.Equal(25, result[2]);
    }

    [Fact]
    public void SubtractInPlace_SeriesSeries_MutatesLeft()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([1, 2, 3]);

        lhs.SubtractInPlace(rhs);

        Assert.Equal(9, lhs[0]);
        Assert.Equal(18, lhs[1]);
        Assert.Equal(27, lhs[2]);
    }

    [Fact]
    public void SubtractInPlace_SeriesScalar_MutatesLeft()
    {
        var series = Series.Create([10, 20, 30]);

        series.SubtractInPlace(5);

        Assert.Equal(5, series[0]);
        Assert.Equal(15, series[1]);
        Assert.Equal(25, series[2]);
    }

    [Fact]
    public void Subtract_LengthMismatch_Throws()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([1, 2]);

        _ = Assert.Throws<NumericsArgumentException>(() => lhs.Subtract(rhs.AsSpan(), new int[3]));
    }
}
