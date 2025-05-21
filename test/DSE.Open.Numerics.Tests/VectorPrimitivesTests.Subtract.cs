// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Subtract_Int32_ToVector()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5];
        Vector<int> v2 = [2, 4, 6, 8, 10];
        Vector<int> difference = new int[5];

        v1.Subtract(v2, difference);

        Assert.Equal([-1, -2, -3, -4, -5], difference);
    }

    [Fact]
    public void Subtract_Int32_ToSpan()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5];
        Vector<int> v2 = [2, 4, 6, 8, 10];

        Span<int> difference = stackalloc int[5];

        v1.Subtract(v2, difference);

        Assert.Equal([-1, -2, -3, -4, -5], difference);
    }

    [Fact]
    public void Subtract_Float32_ToVector()
    {
        Vector<float> v1 = [1, 2, 3, 4, 5];
        Vector<float> v2 = [2, 4, 6, 8, 10];
        Vector<float> difference = new float[5];

        v1.Subtract(v2, difference);

        Assert.Equal([-1, -2, -3, -4, -5], difference);
    }

    [Fact]
    public void Subtract_Float32_ToVector_2()
    {
        Vector<float> v1 = [1, 2, 3, 4, float.NaN];
        Vector<float> v2 = [2, 4, 6, 8, 10];
        Vector<float> difference = new float[5];

        v1.Subtract(v2, difference);

        Assert.Equal([-1, -2, -3, -4, float.NaN], difference);
        Assert.NotEqual([-1, -2, -3, -4, -10], difference);
    }
}
