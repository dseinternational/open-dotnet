// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void GreaterThanOrEqual_SeriesSeries_Int32()
    {
        var left = Series.Create([2, 200, 4000, 30000], "left");
        var right = Series.Create([1, 100, 4000, 40000], "right");
        var result = left.GreaterThanOrEqual(right);
        Assert.Equal([true, true, true, false], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_SeriesSeries_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        Span<bool> destination = stackalloc bool[4];
        left.GreaterThanOrEqual(right, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_SeriesSeries_DestinationSeries()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        var destination = Series.Create<bool>(4);
        left.GreaterThanOrEqual(right, destination);
        Assert.Equal([false, true, true, true], [.. destination]);
    }

    [Fact]
    public void GreaterThanOrEqual_SeriesScalar_Int32()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var result = left.GreaterThanOrEqual(2);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_SeriesScalar_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        left.GreaterThanOrEqual(2, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_ScalarSeries_Int32()
    {
        var right = Series.Create([1, 2, 3, 4]);
        var result = 2.GreaterThanOrEqual(right);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_ScalarSeries_DestinationSpan()
    {
        var right = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        SeriesPrimitives.GreaterThanOrEqual(2, right, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_NullSeries_Throws()
    {
        IReadOnlySeries<int> left = null!;
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<ArgumentNullException>(() => left.GreaterThanOrEqual(right));
    }

    [Fact]
    public void GreaterThanOrEqual_SeriesSeries_DifferentLengths_Throws()
    {
        var left = Series.Create([1, 2, 3]);
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<NumericsArgumentException>(() => left.GreaterThanOrEqual(right));
    }
}
