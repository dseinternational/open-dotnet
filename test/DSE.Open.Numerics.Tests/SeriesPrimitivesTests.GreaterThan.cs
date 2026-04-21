// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void GreaterThan_SeriesSeries_Int32()
    {
        var left = Series.Create([2, 200, 4000, 30000], "left");
        var right = Series.Create([1, 100, 4000, 40000], "right");
        var result = left.GreaterThan(right);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void GreaterThan_SeriesSeries_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        Span<bool> destination = stackalloc bool[4];
        left.GreaterThan(right, destination);
        Assert.Equal([false, false, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThan_SeriesSeries_DestinationSeries()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        var destination = Series.Create<bool>(4);
        left.GreaterThan(right, destination);
        Assert.Equal([false, false, true, true], [.. destination]);
    }

    [Fact]
    public void GreaterThan_SeriesScalar_Int32()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var result = left.GreaterThan(2);
        Assert.Equal([false, false, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThan_SeriesScalar_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        left.GreaterThan(2, destination);
        Assert.Equal([false, false, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThan_ScalarSeries_Int32()
    {
        var right = Series.Create([1, 2, 3, 4]);
        var result = 2.GreaterThan(right);
        Assert.Equal([true, false, false, false], [.. result]);
    }

    [Fact]
    public void GreaterThan_ScalarSeries_DestinationSpan()
    {
        var right = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        SeriesPrimitives.GreaterThan(2, right, destination);
        Assert.Equal([true, false, false, false], destination.ToArray());
    }

    [Fact]
    public void GreaterThan_NullSeries_Throws()
    {
        IReadOnlySeries<int> left = null!;
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<ArgumentNullException>(() => left.GreaterThan(right));
    }

    [Fact]
    public void GreaterThan_SeriesSeries_DifferentLengths_Throws()
    {
        var left = Series.Create([1, 2, 3]);
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<NumericsArgumentException>(() => left.GreaterThan(right));
    }
}
