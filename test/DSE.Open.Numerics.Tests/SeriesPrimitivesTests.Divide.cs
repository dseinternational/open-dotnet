// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void Divide_SeriesSeries_ReturnsQuotient()
    {
        var lhs = Series.Create([10, 20, 30, 40, 50], "a");
        var rhs = Series.Create([2, 4, 5, 8, 10], "b");

        var result = lhs.Divide(rhs);

        Assert.Equal(5, result[0]);
        Assert.Equal(5, result[1]);
        Assert.Equal(6, result[2]);
        Assert.Equal(5, result[3]);
        Assert.Equal(5, result[4]);
    }

    [Fact]
    public void Divide_SeriesSeries_IntoSpan_WritesQuotient()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([2, 4, 5]);
        Span<int> destination = stackalloc int[3];

        lhs.Divide(rhs, destination);

        Assert.Equal(5, destination[0]);
        Assert.Equal(5, destination[1]);
        Assert.Equal(6, destination[2]);
    }

    [Fact]
    public void Divide_SeriesSeries_IntoRhsSpan_WritesQuotient()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([2, 4, 5]);

        lhs.Divide(rhs, rhs.AsSpan());

        Assert.Equal(5, rhs[0]);
        Assert.Equal(5, rhs[1]);
        Assert.Equal(6, rhs[2]);
    }

    [Fact]
    public void Divide_SeriesScalar_ReturnsQuotient()
    {
        var series = Series.Create([10, 20, 30]);

        var result = series.Divide(2);

        Assert.Equal(5, result[0]);
        Assert.Equal(10, result[1]);
        Assert.Equal(15, result[2]);
    }

    [Fact]
    public void DivideInPlace_SeriesSeries_MutatesLeft()
    {
        var lhs = Series.Create([10, 20, 30]);
        var rhs = Series.Create([2, 4, 5]);

        lhs.DivideInPlace(rhs);

        Assert.Equal(5, lhs[0]);
        Assert.Equal(5, lhs[1]);
        Assert.Equal(6, lhs[2]);
    }

    [Fact]
    public void DivideInPlace_SeriesScalar_MutatesLeft()
    {
        var series = Series.Create([10, 20, 30]);

        series.DivideInPlace(5);

        Assert.Equal(2, series[0]);
        Assert.Equal(4, series[1]);
        Assert.Equal(6, series[2]);
    }

    [Fact]
    public void Divide_LengthMismatch_Throws()
    {
        var lhs = Series.Create([1, 2, 3]);
        var rhs = Series.Create([1, 2]);

        _ = Assert.Throws<NumericsArgumentException>(() => lhs.Divide(rhs.AsSpan(), new int[3]));
    }
}
