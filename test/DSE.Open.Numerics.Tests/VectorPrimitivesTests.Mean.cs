// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Mean_Span_Int32_Int32()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = VectorPrimitives.Mean<int, int>(sequence);
        Assert.Equal(5, mean);
    }

    [Fact]
    public void Mean_Span_Int32_Double()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = VectorPrimitives.Mean<int, double>(sequence);
        Assert.Equal(5.5, mean);
    }
}
