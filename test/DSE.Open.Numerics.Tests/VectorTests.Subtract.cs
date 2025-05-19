// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Subtract_Int32_Array_Int32()
    {
        int[] s1 = [1, 2, 3, 4, 5];
        int[] s2 = [1, 2, 3, 4, 5];
        var difference = new int[5];

        Vector.Subtract(s1, s2, difference);

        Assert.Equal(0, difference[0]);
        Assert.Equal(0, difference[1]);
        Assert.Equal(0, difference[2]);
        Assert.Equal(0, difference[3]);
        Assert.Equal(0, difference[4]);
    }

    [Fact]
    public void Subtract_Int32_Span_Int32()
    {
        Span<int> s1 = [1, 2, 3, 4, 5];
        Span<int> s2 = [1, 2, 3, 4, 5];

        Span<int> difference = stackalloc int[5];

        Vector.Subtract(s1, s2, difference);

        Assert.Equal(0, difference[0]);
        Assert.Equal(0, difference[1]);
        Assert.Equal(0, difference[2]);
        Assert.Equal(0, difference[3]);
        Assert.Equal(0, difference[4]);
    }
}
