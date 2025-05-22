// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void IsNan_Float32()
    {
        Vector<float> vector = [1, 2, 3, float.NaN, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaFloat32()
    {
        Vector<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaInt32()
    {
        Vector<NaInt<int>> vector = [1, 2, 3, null, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_Float32_Vector()
    {
        Vector<float> vector = [1, 2, 3, float.NaN, 5, 6];
        var result = Vector.Create<bool>(6);

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaFloat32_Vector()
    {
        Vector<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        var result = Vector.Create<bool>(6);

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaInt32_Vector()
    {
        Vector<NaInt<int>> vector = [1, 2, 3, null, 5, 6];
        var result = Vector.Create<bool>(6);

        VectorPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNaNAll_Float32()
    {
        Vector<float> vector = [float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN];
        Assert.True(VectorPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_Float32_False()
    {
        Vector<float> vector = [1, 2, 3, float.NaN, 5, 6];
        Assert.False(VectorPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_NaFloat32()
    {
        Vector<NaFloat<float>> vector = [float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN];
        Assert.True(VectorPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_NaFloat32_False()
    {
        Vector<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        Assert.False(VectorPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAny_Float32()
    {
        Vector<float> vector = [1, 2, 3, 4, 5, 6, float.NaN];
        Assert.True(VectorPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_Float32_False()
    {
        Vector<float> vector = [1, 2, 3, 4, 5, 6];
        Assert.False(VectorPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_NaFloat32()
    {
        Vector<NaFloat<float>> vector = [1, 2, 3, 4, 5, 6, float.NaN];
        Assert.True(VectorPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_NaFloat32_False()
    {
        Vector<NaFloat<float>> vector = [1, 2, 3, 4, 5, 6];
        Assert.False(VectorPrimitives.IsNaNAny(vector));
    }
}
