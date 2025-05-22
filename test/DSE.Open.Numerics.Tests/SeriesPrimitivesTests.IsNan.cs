// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTestsIsNan
{
    [Fact]
    public void IsNan_Float32()
    {
        Series<float> vector = [1, 2, 3, float.NaN, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        SeriesPrimitives.IsNaN(vector, result);

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
        Series<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        SeriesPrimitives.IsNaN(vector, result);

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
        Series<NaInt<int>> vector = [1, 2, 3, null, 5, 6];
        Span<bool> result = stackalloc bool[vector.Length];

        SeriesPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_Float32_Series()
    {
        Series<float> vector = [1, 2, 3, float.NaN, 5, 6];
        var result = Series.Create<bool>(6);

        SeriesPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaFloat32_Series()
    {
        Series<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        var result = Series.Create<bool>(6);

        SeriesPrimitives.IsNaN(vector, result);

        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.False(result[2]);
        Assert.True(result[3]);
        Assert.False(result[4]);
        Assert.False(result[5]);
    }

    [Fact]
    public void IsNan_NaInt32_Series()
    {
        Series<NaInt<int>> vector = [1, 2, 3, null, 5, 6];
        var result = Series.Create<bool>(6);

        SeriesPrimitives.IsNaN(vector, result);

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
        Series<float> vector = [float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN];
        Assert.True(SeriesPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_Float32_False()
    {
        Series<float> vector = [1, 2, 3, float.NaN, 5, 6];
        Assert.False(SeriesPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_NaFloat32()
    {
        Series<NaFloat<float>> vector = [float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN];
        Assert.True(SeriesPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAll_NaFloat32_False()
    {
        Series<NaFloat<float>> vector = [1, 2, 3, float.NaN, 5, 6];
        Assert.False(SeriesPrimitives.IsNaNAll(vector));
    }

    [Fact]
    public void IsNaNAny_Float32()
    {
        Series<float> vector = [1, 2, 3, 4, 5, 6, float.NaN];
        Assert.True(SeriesPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_Float32_False()
    {
        Series<float> vector = [1, 2, 3, 4, 5, 6];
        Assert.False(SeriesPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_NaFloat32()
    {
        Series<NaFloat<float>> vector = [1, 2, 3, 4, 5, 6, float.NaN];
        Assert.True(SeriesPrimitives.IsNaNAny(vector));
    }

    [Fact]
    public void IsNaNAny_NaFloat32_False()
    {
        Series<NaFloat<float>> vector = [1, 2, 3, 4, 5, 6];
        Assert.False(SeriesPrimitives.IsNaNAny(vector));
    }
}
