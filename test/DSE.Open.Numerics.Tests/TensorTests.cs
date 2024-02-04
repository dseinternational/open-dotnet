// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public class TensorTests
{
    [Fact]
    public void Init_2D_Array()
    {
        var t = new Tensor<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        });

        Assert.Equal(2u, t.Rank);
        Assert.Equal(6u, t.Length);
        Assert.Equal(3u, t.Shape[0]);
        Assert.Equal(2u, t.Shape[1]);
        Assert.Equal(1, t[[0, 0]]);
        Assert.Equal(2, t[[0, 1]]);
        Assert.Equal(3, t[[1, 0]]);
        Assert.Equal(4, t[[1, 1]]);
    }
}
