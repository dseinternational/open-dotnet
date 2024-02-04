// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public class ReadOnlyTensorSpanTests
{
    [Fact]
    public void Init_2D_Array()
    {
        var t = new ReadOnlyTensorSpan<int>(new int[,]
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

    [Fact]
    public void Add_2D_Array()
    {
        var t1 = new ReadOnlyTensorSpan<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        });

        var t2 = new ReadOnlyTensorSpan<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        });

        var t3 = t1.Add(t2);

        Assert.Equal(2u, t3.Rank);
        Assert.Equal(6u, t3.Length);
        Assert.Equal(2, t3[[0, 0]]);
        Assert.Equal(4, t3[[0, 1]]);
        Assert.Equal(6, t3[[1, 0]]);
        Assert.Equal(8, t3[[1, 1]]);
    }
}

