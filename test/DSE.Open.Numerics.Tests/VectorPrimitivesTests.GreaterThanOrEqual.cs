// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void GreaterThanOrEqual_VectorVector_ReturnsTrue_WhenStrictlyGreater()
    {
        var v1 = Vector.Create([2, 200, 4000, 30000]);
        var v2 = Vector.Create([1, 100, 2000, 10000]);
        var result = v1.GreaterThanOrEqual(v2);
        Assert.Equal([true, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_VectorVector_ReturnsTrue_WhenEqual()
    {
        var v1 = Vector.Create([1, 100, 2000, 10000]);
        var v2 = Vector.Create([1, 100, 2000, 10000]);
        var result = v1.GreaterThanOrEqual(v2);
        Assert.Equal([true, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_VectorVector_ReturnsFalse_WhenStrictlyLess()
    {
        var v1 = Vector.Create([2, 200, 4000, 30000]);
        var v2 = Vector.Create([3, 300, 5000, 40000]);
        var result = v1.GreaterThanOrEqual(v2);
        Assert.Equal([false, false, false, false], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_VectorVector_MixedResults()
    {
        var v1 = Vector.Create([1, 2, 3, 4]);
        var v2 = Vector.Create([2, 2, 2, 2]);
        var result = v1.GreaterThanOrEqual(v2);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_VectorVector_Destination()
    {
        var v1 = Vector.Create([1, 2, 3, 4]);
        var v2 = Vector.Create([2, 2, 2, 2]);
        var destination = Vector.CreateUninitialized<bool>(4);
        var result = v1.GreaterThanOrEqual(v2, destination);
        Assert.Equal([false, true, true, true], [.. result]);
        Assert.Equal([false, true, true, true], [.. destination]);
    }

    [Fact]
    public void GreaterThanOrEqual_VectorScalar_MixedResults()
    {
        var v = Vector.Create([1, 2, 3, 4]);
        var result = v.GreaterThanOrEqual(2);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_ScalarVector_MixedResults()
    {
        var v = Vector.Create([1, 2, 3, 4]);
        var result = 2.GreaterThanOrEqual(v);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_SpanSpan_WritesToDestination()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        ReadOnlySpan<int> y = [2, 2, 2, 2];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.GreaterThanOrEqual(x, y, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_SpanScalar_WritesToDestination()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.GreaterThanOrEqual(x, 2, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_ScalarSpan_WritesToDestination()
    {
        ReadOnlySpan<int> y = [1, 2, 3, 4];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.GreaterThanOrEqual(2, y, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void GreaterThanOrEqual_Double_MixedResults()
    {
        var v1 = Vector.Create([1.0, 2.0, 3.0, 4.0]);
        var v2 = Vector.Create([2.0, 2.0, 2.0, 2.0]);
        var result = v1.GreaterThanOrEqual(v2);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThanOrEqual_NullX_Throws()
    {
        IReadOnlyVector<int> x = null!;
        var y = Vector.Create([1, 2]);
        _ = Assert.Throws<ArgumentNullException>(() => x.GreaterThanOrEqual(y));
    }

    [Fact]
    public void GreaterThanOrEqual_NullY_Throws()
    {
        var x = Vector.Create([1, 2]);
        IReadOnlyVector<int> y = null!;
        _ = Assert.Throws<ArgumentNullException>(() => x.GreaterThanOrEqual(y));
    }

    [Fact]
    public void GreaterThanOrEqual_LengthMismatch_Throws()
    {
        var x = Vector.Create([1, 2, 3]);
        var y = Vector.Create([1, 2]);
        _ = Assert.Throws<NumericsArgumentException>(() => x.GreaterThanOrEqual(y));
    }
}
