// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

public class NumericMatrixPrimitivesAddTests
{
    [Fact]
    public void Add_MatrixScalar_ContiguousInputs_ReturnsElementwiseSum()
    {
        Span<int> xBuffer = [1, 2, 3, 4];
        Span<int> destBuffer = stackalloc int[4];

        var x = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 0);
        var destination = Span2D<int>.DangerousCreate(ref destBuffer[0], 2, 2, 0);

        NumericMatrixPrimitives.Add(x, 10, destination);

        Assert.Equal(11, destination[0, 0]);
        Assert.Equal(12, destination[0, 1]);
        Assert.Equal(13, destination[1, 0]);
        Assert.Equal(14, destination[1, 1]);
    }

    [Fact]
    public void Add_MatrixMatrix_ContiguousInputs_ReturnsElementwiseSum()
    {
        Span<int> xBuffer = [1, 2, 3, 4];
        Span<int> yBuffer = [10, 20, 30, 40];
        Span<int> destBuffer = stackalloc int[4];

        var x = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 0);
        var y = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref yBuffer[0], 2, 2, 0);
        var destination = Span2D<int>.DangerousCreate(ref destBuffer[0], 2, 2, 0);

        NumericMatrixPrimitives.Add(x, y, destination);

        Assert.Equal(11, destination[0, 0]);
        Assert.Equal(22, destination[0, 1]);
        Assert.Equal(33, destination[1, 0]);
        Assert.Equal(44, destination[1, 1]);
    }

    [Fact]
    public void Add_MatrixMatrix_NonContiguousInputs_ReturnsElementwiseSum()
    {
        // 3-wide backing viewed as 2x2 with pitch 1 — TryGetSpan returns false,
        // exercising the row-by-row fallback path.
        Span<int> xBuffer = [
            1, 2, 99,
            3, 4, 99,
        ];
        Span<int> yBuffer = [
            10, 20, 99,
            30, 40, 99,
        ];
        Span<int> destBuffer = stackalloc int[6];

        var x = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 1);
        var y = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref yBuffer[0], 2, 2, 1);
        var destination = Span2D<int>.DangerousCreate(ref destBuffer[0], 2, 2, 1);

        Assert.False(x.TryGetSpan(out _));

        NumericMatrixPrimitives.Add(x, y, destination);

        Assert.Equal(11, destination[0, 0]);
        Assert.Equal(22, destination[0, 1]);
        Assert.Equal(33, destination[1, 0]);
        Assert.Equal(44, destination[1, 1]);
    }

    [Fact]
    public void AddInPlace_MatrixScalar_MutatesInput()
    {
        Span<int> buffer = [1, 2, 3, 4];
        var x = Span2D<int>.DangerousCreate(ref buffer[0], 2, 2, 0);

        NumericMatrixPrimitives.AddInPlace(x, 5);

        Assert.Equal(6, x[0, 0]);
        Assert.Equal(7, x[0, 1]);
        Assert.Equal(8, x[1, 0]);
        Assert.Equal(9, x[1, 1]);
    }

    [Fact]
    public void AddInPlace_MatrixMatrix_MutatesInput()
    {
        Span<int> xBuffer = [1, 2, 3, 4];
        Span<int> yBuffer = [10, 20, 30, 40];
        var x = Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 0);
        var y = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref yBuffer[0], 2, 2, 0);

        NumericMatrixPrimitives.AddInPlace(x, y);

        Assert.Equal(11, x[0, 0]);
        Assert.Equal(22, x[0, 1]);
        Assert.Equal(33, x[1, 0]);
        Assert.Equal(44, x[1, 1]);
    }
}
