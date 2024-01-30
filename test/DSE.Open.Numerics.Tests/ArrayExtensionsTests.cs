// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class ArrayExtensionsTests
{
    [Fact]
    public void AsArray2D_Array_Int32()
    {
        int[] data = [1, 2, 3, 4, 5, 6];
        var array = data.ToArray2D(2, 3);
        Assert.Equal(2, array.GetLength(0));
        Assert.Equal(3, array.GetLength(1));
        Assert.Equal(1, array[0, 0]);
        Assert.Equal(2, array[0, 1]);
        Assert.Equal(3, array[0, 2]);
        Assert.Equal(4, array[1, 0]);
        Assert.Equal(5, array[1, 1]);
        Assert.Equal(6, array[1, 2]);
    }

    [Fact]
    public void AsArray2D_Array_Span_Int32()
    {
        Span<int> data = [1, 2, 3, 4, 5, 6];
        var array = data.ToArray2D(2, 3);
        Assert.Equal(2, array.GetLength(0));
        Assert.Equal(3, array.GetLength(1));
        Assert.Equal(1, array[0, 0]);
        Assert.Equal(2, array[0, 1]);
        Assert.Equal(3, array[0, 2]);
        Assert.Equal(4, array[1, 0]);
        Assert.Equal(5, array[1, 1]);
        Assert.Equal(6, array[1, 2]);
    }
}
