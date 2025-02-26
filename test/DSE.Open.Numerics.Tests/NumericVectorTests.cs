// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class NumericVectorTests
{
    [Fact]
    public void Init()
    {
        NumericVector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = new NumericVector<int>([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.Span.SequenceEqual(v2.Span));
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = NumericVector.CreateDefault<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = NumericVector.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = NumericVector.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual([1, 1, 1, 1, 1, 1]));
    }
}
