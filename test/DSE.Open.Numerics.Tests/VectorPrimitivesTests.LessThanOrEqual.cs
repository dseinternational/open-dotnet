// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void LessThanOrEqual_VectorVector_ReturnsTrue_WhenStrictlyLess()
    {
        var v1 = Vector.Create([1, 100, 2000, 10000]);
        var v2 = Vector.Create([2, 200, 4000, 30000]);
        var result = v1.LessThanOrEqual(v2);
        Assert.Equal([true, true, true, true], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_VectorVector_ReturnsTrue_WhenEqual()
    {
        var v1 = Vector.Create([1, 100, 2000, 10000]);
        var v2 = Vector.Create([1, 100, 2000, 10000]);
        var result = v1.LessThanOrEqual(v2);
        Assert.Equal([true, true, true, true], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_VectorVector_ReturnsFalse_WhenStrictlyGreater()
    {
        var v1 = Vector.Create([3, 300, 5000, 40000]);
        var v2 = Vector.Create([2, 200, 4000, 30000]);
        var result = v1.LessThanOrEqual(v2);
        Assert.Equal([false, false, false, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_VectorVector_MixedResults()
    {
        var v1 = Vector.Create([1, 2, 3, 4]);
        var v2 = Vector.Create([2, 2, 2, 2]);
        var result = v1.LessThanOrEqual(v2);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_VectorVector_Destination()
    {
        var v1 = Vector.Create([1, 2, 3, 4]);
        var v2 = Vector.Create([2, 2, 2, 2]);
        var destination = Vector.CreateUninitialized<bool>(4);
        var result = v1.LessThanOrEqual(v2, destination);
        Assert.Equal([true, true, false, false], [.. result]);
        Assert.Equal([true, true, false, false], [.. destination]);
    }

    [Fact]
    public void LessThanOrEqual_VectorScalar_MixedResults()
    {
        var v = Vector.Create([1, 2, 3, 4]);
        var result = v.LessThanOrEqual(2);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_ScalarVector_MixedResults()
    {
        var v = Vector.Create([1, 2, 3, 4]);
        var result = 2.LessThanOrEqual(v);
        Assert.Equal([false, true, true, true], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_SpanSpan_WritesToDestination()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        ReadOnlySpan<int> y = [2, 2, 2, 2];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.LessThanOrEqual(x, y, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_SpanScalar_WritesToDestination()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.LessThanOrEqual(x, 2, destination);
        Assert.Equal([true, true, false, false], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_ScalarSpan_WritesToDestination()
    {
        ReadOnlySpan<int> y = [1, 2, 3, 4];
        Span<bool> destination = stackalloc bool[4];
        _ = VectorPrimitives.LessThanOrEqual(2, y, destination);
        Assert.Equal([false, true, true, true], destination.ToArray());
    }

    [Fact]
    public void LessThanOrEqual_Double_MixedResults()
    {
        var v1 = Vector.Create([1.0, 2.0, 3.0, 4.0]);
        var v2 = Vector.Create([2.0, 2.0, 2.0, 2.0]);
        var result = v1.LessThanOrEqual(v2);
        Assert.Equal([true, true, false, false], [.. result]);
    }

    [Fact]
    public void LessThanOrEqual_NullX_Throws()
    {
        IReadOnlyVector<int> x = null!;
        var y = Vector.Create([1, 2]);
        _ = Assert.Throws<ArgumentNullException>(() => x.LessThanOrEqual(y));
    }

    [Fact]
    public void LessThanOrEqual_NullY_Throws()
    {
        var x = Vector.Create([1, 2]);
        IReadOnlyVector<int> y = null!;
        _ = Assert.Throws<ArgumentNullException>(() => x.LessThanOrEqual(y));
    }

    [Fact]
    public void LessThanOrEqual_LengthMismatch_Throws()
    {
        var x = Vector.Create([1, 2, 3]);
        var y = Vector.Create([1, 2]);
        _ = Assert.Throws<NumericsArgumentException>(() => x.LessThanOrEqual(y));
    }
}
