// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    [Fact]
    public void LessThanOrEqual_SeriesSeries_Int32()
    {
        var left = Series.Create([1, 100, 4000, 40000], "left");
        var right = Series.Create([2, 200, 4000, 30000], "right");
        var result = left.LessThanOrEqual(right);
        Assert.Equal([true, true, true, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_SeriesSeries_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        Span<bool> destination = stackalloc bool[4];
        left.LessThanOrEqual(right, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_SeriesSeries_DestinationSeries()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var right = Series.Create([2, 2, 2, 2]);
        var destination = Series.Create<bool>(4);
        left.LessThanOrEqual(right, destination);
        Assert.Equal([true, true, false, false], [.. destination]);
    }

    [Fact]
    public void LessThanOrEqual_SeriesScalar_Int32()
    {
        var left = Series.Create([1, 2, 3, 4]);
        var result = left.LessThanOrEqual(2);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_SeriesScalar_DestinationSpan()
    {
        var left = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        left.LessThanOrEqual(2, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_ScalarSeries_Int32()
    {
        var right = Series.Create([1, 2, 3, 4]);
        var result = 2.LessThanOrEqual(right);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_ScalarSeries_DestinationSpan()
    {
        var right = Series.Create([1, 2, 3, 4]);
        Span<bool> destination = stackalloc bool[4];
        SeriesPrimitives.LessThanOrEqual(2, right, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_NullSeries_Throws()
    {
        IReadOnlySeries<int> left = null!;
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<ArgumentNullException>(() => left.LessThanOrEqual(right));
    }

    [Fact]
    public void LessThanOrEqual_SeriesSeries_DifferentLengths_Throws()
    {
        var left = Series.Create([1, 2, 3]);
        var right = Series.Create([1, 2]);
        _ = Assert.Throws<NumericsArgumentException>(() => left.LessThanOrEqual(right));
    }
}
