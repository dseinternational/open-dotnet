// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTestsVariance
{
    [Fact]
    public void Variance_Span_Double()
    {
        ReadOnlySpan<double> sequence = [1, 2, 3, 4, 5];
        var variance = VectorPrimitives.Variance(sequence);
        Assert.Equal(2.5, variance);
    }

    [Fact]
    public void Variance_Span_Double_2()
    {
        ReadOnlySpan<double> sequence = [10, 12, 23, 23, 16, 23, 21, 16];
        var variance = VectorPrimitives.Variance(sequence);
        Assert.Equal(27.428571428571, variance, 0.000000000001);
    }
}
