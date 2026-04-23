// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void Multiply_SeriesSeries_ReturnsProduct()
    {
        var lhs = Series.Create([1, 2, 3, 4, 5], "a");
        var rhs = Series.Create([2, 2, 2, 2, 2], "b");

        var result = lhs.Multiply(rhs);

        Assert.Equal(2, result[0]);
        Assert.Equal(4, result[1]);
        Assert.Equal(6, result[2]);
        Assert.Equal(8, result[3]);
        Assert.Equal(10, result[4]);
    }

    [Fact]
    public void Multiply_SeriesSeries_IntoSpan_WritesProduct()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([4, 5, 6]);
        Span<int> destination = stackalloc int[3];

        lhs.Multiply(rhs, destination);

        Assert.Equal(4, destination[0]);
        Assert.Equal(10, destination[1]);
        Assert.Equal(18, destination[2]);
    }

    [Fact]
    public void Multiply_SeriesScalar_ReturnsProduct()
    {
        var series = Series.Create([1, 2, 3]);

        var result = series.Multiply(3);

        Assert.Equal(3, result[0]);
        Assert.Equal(6, result[1]);
        Assert.Equal(9, result[2]);
    }

    [Fact]
    public void MultiplyInPlace_SeriesSeries_MutatesLeft()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([2, 4, 6]);

        lhs.MultiplyInPlace(rhs);

        Assert.Equal(2, lhs[0]);
        Assert.Equal(8, lhs[1]);
        Assert.Equal(18, lhs[2]);
    }

    [Fact]
    public void MultiplyInPlace_SeriesScalar_MutatesLeft()
    {
        var series = Series.Create([1, 2, 3]);

        series.MultiplyInPlace(4);

        Assert.Equal(4, series[0]);
        Assert.Equal(8, series[1]);
        Assert.Equal(12, series[2]);
    }

    [Fact]
    public void Multiply_LengthMismatch_Throws()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([1, 2]);

        _ = Assert.Throws<NumericsArgumentException>(() => lhs.Multiply(rhs.AsSpan(), new int[3]));
    }
}
