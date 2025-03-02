// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class NumericVectorTests
{
    [Fact]
    public void Init()
    {
        NumericVector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = Vector.CreateNumeric([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.Span.SequenceEqual(v2.Span));
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = Vector.CreateNumeric<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = Vector.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Span.SequenceEqual([1, 1, 1, 1, 1, 1]));
    }

    [Fact]
    public void Equality()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void AdditionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 + v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.Span.SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void AdditionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 + 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.Span.SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void SubtractionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 - v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.Span.SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void SubtractionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 - 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.Span.SequenceEqual([0, 0, 0, 0, 0, 0]));
    }
}
