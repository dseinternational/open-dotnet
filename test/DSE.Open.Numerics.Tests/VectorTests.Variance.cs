// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Variance_Span_Double()
    {
        ReadOnlySpan<double> sequence = [1, 2, 3, 4, 5];
        var variance = Vector.Variance(sequence);
        Assert.Equal(2.5, variance);
    }

    [Fact]
    public void Variance_Span_Double_2()
    {
        ReadOnlySpan<double> sequence = [10, 12, 23, 23, 16, 23, 21, 16];
        var variance = Vector.Variance(sequence);
        Assert.Equal(27.428571428571, variance, 0.000000000001);
    }

    [Fact]
    public void Variance_Span_Int32_Double()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5];
        var variance = Vector.Variance<int, double>(sequence);
        Assert.Equal(2.5, variance);
    }

    [Fact]
    public void Variance_Span_Int64_Double()
    {
        ReadOnlySpan<long> sequence = [1, 2, 3, 4, 5];
        var variance = Vector.Variance<long, double>(sequence);
        Assert.Equal(2.5, variance);
    }
}
