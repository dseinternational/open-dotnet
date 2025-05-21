// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Equals_X_Y()
    {
        var result = Vector.Equals([1, 2, 3], [1, 2, 3]);
        Assert.True(result[0]);
        Assert.True(result[1]);
        Assert.True(result[2]);
    }

    [Fact]
    public void Equals_X_Y_False()
    {
        var result = Vector.Equals([1, 2, 3], [4, 5, 6]);
        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Dest()
    {
        Span<bool> dest = stackalloc bool[3];
        var result = Vector.Equals([1, 2, 3], [1, 2, 3], dest);
        Assert.True(dest[0]);
        Assert.True(dest[1]);
        Assert.True(dest[2]);
        Assert.True(result[0]);
        Assert.True(result[1]);
        Assert.True(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Dest_False()
    {
        Span<bool> dest = stackalloc bool[3];
        var result = Vector.Equals([1, 2, 3], [4, 5, 6], dest);
        Assert.False(dest[0]);
        Assert.False(dest[1]);
        Assert.False(dest[2]);
        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Scalar()
    {
        var result = Vector.Equals([3, 3, 3], 3);
        Assert.True(result[0]);
        Assert.True(result[1]);
        Assert.True(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Scalar_False()
    {
        var result = Vector.Equals([3, 3, 3], 4);
        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Scalar_Dest()
    {
        Span<bool> dest = stackalloc bool[3];
        var result = Vector.Equals([5, 5, 5], 5, dest);
        Assert.True(dest[0]);
        Assert.True(dest[1]);
        Assert.True(dest[2]);
        Assert.True(result[0]);
        Assert.True(result[1]);
        Assert.True(result[2]);
    }

    [Fact]
    public void Equals_X_Y_Scalar_Dest_False()
    {
        Span<bool> dest = stackalloc bool[3];
        var result = Vector.Equals([7, 7, 7], 1, dest);
        Assert.False(dest[0]);
        Assert.False(dest[1]);
        Assert.False(dest[2]);
        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
    }
}
