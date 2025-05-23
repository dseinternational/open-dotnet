// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTestsDivide
{
    [Fact]
    public void Divide_VectorAndSpan_ReturnsCorrectResult()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        ReadOnlySpan<int> y = [2, 4, 5, 8, 10];
        Span<int> destination = stackalloc int[5];

        x.Divide(y, destination);

        Assert.True(destination.SequenceEqual([5, 5, 6, 5, 5]));
    }

    [Fact]
    public void Divide_TwoVectors_ReturnsCorrectResult()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        Vector<int> y = [2, 4, 5, 8, 10];
        Span<int> destination = stackalloc int[5];

        x.Divide(y, destination);

        Assert.True(destination.SequenceEqual([5, 5, 6, 5, 5]));
    }

    [Fact]
    public void Divide_VectorAndScalar_ReturnsCorrectResult()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        var scalar = 10;
        Span<int> destination = stackalloc int[5];

        x.Divide(scalar, destination);

        Assert.True(destination.SequenceEqual([1, 2, 3, 4, 5]));
    }
    [Fact]
    public void Divide_VectorAndScalar_ReturnsNewVector()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        var scalar = 10;

        var result = x.Divide(scalar);

        Assert.Equal([1, 2, 3, 4, 5], [.. result]);
    }

    [Fact]
    public void DivideInPlace_VectorAndSpan_ModifiesVector()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        ReadOnlySpan<int> y = [2, 4, 5, 8, 10];

        x.DivideInPlace(y);

        Assert.Equal([5, 5, 6, 5, 5], [.. x]);
    }

    [Fact]
    public void DivideInPlace_VectorAndScalar_ModifiesVector()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        var scalar = 10;

        x.DivideInPlace(scalar);

        Assert.Equal([1, 2, 3, 4, 5], [.. x]);
    }

    [Fact]
    public void Divide_EmptyVectors_ReturnsEmptyResult()
    {
        var x = Vector<int>.Empty;
        ReadOnlySpan<int> y = [];
        Span<int> destination = [];

        x.Divide(y, destination);

        Assert.True(destination.IsEmpty);
    }

    [Fact]
    public void Divide_MismatchedLengths_ThrowsException()
    {
        Vector<int> x = [10, 20, 30];
        ReadOnlySpan<int> y = [2, 4];

        Span<int> destination = stackalloc int[3];

        var thrown = false;

        try
        {
            x.Divide(y, destination);
        }
        catch (NumericsArgumentException)
        {
            thrown = true;
        }

        Assert.True(thrown, "Expected NumericsException was not thrown.");
    }

    [Fact]
    public void Divide_ByZero_ThrowsException()
    {
        Vector<int> x = [10, 20, 30, 40, 50];
        ReadOnlySpan<int> y = [2, 0, 5, 8, 10];
        Span<int> destination = stackalloc int[5];

        var thrown = false;

        try
        {
            x.Divide(y, destination);
        }
        catch (DivideByZeroException)
        {
            thrown = true;
        }

        Assert.True(thrown, "Expected DivideByZeroException was not thrown.");
    }

    [Fact]
    public void Divide_NullVector_ThrowsArgumentNullException()
    {
        IReadOnlyVector<int>? x = null;
        ReadOnlySpan<int> y = [2, 3, 4];
        Span<int> destination = stackalloc int[3];

        var thrown = false;

        try
        {
            x!.Divide(y, destination);
        }
        catch (ArgumentNullException)
        {
            thrown = true;
        }

        Assert.True(thrown, "Expected ArgumentNullException was not thrown.");
    }
}
