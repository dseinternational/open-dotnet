// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Multiply_VectorAndSpan_ReturnsCorrectResult()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> y = [2, 3, 4, 5, 6];
        Span<int> destination = stackalloc int[5];

        x.Multiply(y, destination);

        Assert.True(destination.SequenceEqual([2, 6, 12, 20, 30]));
    }

    [Fact]
    public void Multiply_TwoVectors_ReturnsCorrectResult()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        Vector<int> y = [2, 3, 4, 5, 6];
        Span<int> destination = stackalloc int[5];

        x.Multiply(y, destination);

        Assert.True(destination.SequenceEqual([2, 6, 12, 20, 30]));
    }

    [Fact]
    public void Multiply_VectorAndScalar_ReturnsCorrectResult()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        var scalar = 3;
        Span<int> destination = stackalloc int[5];

        x.Multiply(scalar, destination);

        Assert.True(destination.SequenceEqual([3, 6, 9, 12, 15]));
    }

    [Fact]
    public void Multiply_VectorAndScalar_ReturnsNewVector()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        var scalar = 3;

        var result = x.Multiply(scalar);

        Assert.Equal([3, 6, 9, 12, 15], [.. result]);
    }

    [Fact]
    public void MultiplyInPlace_VectorAndSpan_ModifiesVector()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> y = [2, 3, 4, 5, 6];

        x.MultiplyInPlace(y);

        Assert.Equal([2, 6, 12, 20, 30], [.. x]);
    }

    [Fact]
    public void MultiplyInPlace_VectorAndScalar_ModifiesVector()
    {
        Vector<int> x = [1, 2, 3, 4, 5];
        var scalar = 3;

        x.MultiplyInPlace(scalar);

        Assert.Equal([3, 6, 9, 12, 15], [.. x]);
    }

    [Fact]
    public void Multiply_EmptyVectors_ReturnsEmptyResult()
    {
        var x = Vector<int>.Empty;
        ReadOnlySpan<int> y = [];
        Span<int> destination = [];

        x.Multiply(y, destination);

        Assert.True(destination.IsEmpty);
    }

    [Fact]
    public void Multiply_MismatchedLengths_ThrowsException()
    {
        Vector<int> x = [1, 2, 3];
        ReadOnlySpan<int> y = [2, 3];

        Span<int> destination = stackalloc int[3];

        var thrown = false;

        try
        {
            x.Multiply(y, destination);
        }
        catch (NumericsException)
        {
            thrown = true;
        }

        Assert.True(thrown);
    }

    [Fact]
    public void Multiply_NullVector_ThrowsArgumentNullException()
    {
        IReadOnlyVector<int>? x = null;
        ReadOnlySpan<int> y = [2, 3, 4];
        Span<int> destination = stackalloc int[3];

        var thrown = false;

        try
        {
            x!.Multiply(y, destination);
        }
        catch (ArgumentNullException)
        {
            thrown = true;
        }

        Assert.True(thrown);
    }
}
