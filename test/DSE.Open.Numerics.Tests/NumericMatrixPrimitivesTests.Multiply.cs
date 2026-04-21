// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

public class NumericMatrixPrimitivesMultiplyTests
{
    [Fact]
    public void Multiply_ContiguousInputs_ReturnsElementwiseProduct()
    {
        Span<int> xBuffer = [1, 2, 3, 4];
        Span<int> yBuffer = [5, 6, 7, 8];
        Span<int> destBuffer = stackalloc int[4];

        var x = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 0);
        var y = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref yBuffer[0], 2, 2, 0);
        var destination = Span2D<int>.DangerousCreate(ref destBuffer[0], 2, 2, 0);

        NumericMatrixPrimitives.Multiply(x, y, destination);

        Assert.Equal(5, destination[0, 0]);
        Assert.Equal(12, destination[0, 1]);
        Assert.Equal(21, destination[1, 0]);
        Assert.Equal(32, destination[1, 1]);
    }

    [Fact]
    public void Multiply_NonContiguousInputs_ReturnsElementwiseProduct()
    {
        // 3-wide backing, viewed as 2x2 with pitch 1 — so TryGetSpan returns false
        // and the row-by-row fallback path is exercised.
        Span<int> xBuffer = [
            1, 2, 99,
            3, 4, 99,
        ];
        Span<int> yBuffer = [
            5, 6, 99,
            7, 8, 99,
        ];
        Span<int> destBuffer = stackalloc int[6];

        var x = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref xBuffer[0], 2, 2, 1);
        var y = (ReadOnlySpan2D<int>)Span2D<int>.DangerousCreate(ref yBuffer[0], 2, 2, 1);
        var destination = Span2D<int>.DangerousCreate(ref destBuffer[0], 2, 2, 1);

        Assert.False(x.TryGetSpan(out _));

        NumericMatrixPrimitives.Multiply(x, y, destination);

        Assert.Equal(5, destination[0, 0]);
        Assert.Equal(12, destination[0, 1]);
        Assert.Equal(21, destination[1, 0]);
        Assert.Equal(32, destination[1, 1]);
    }
}
