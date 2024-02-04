// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public class MemoryNDTests
{
    [Fact]
    public void Create_2D_Array()
    {
        var data = new int[,] { { 1, 2 }, { 3, 4 } };

        var t = MemoryND.Create(data);

        Assert.Equal(2, t.Shape.Length);
        Assert.Equal(4u, t.Length);
        Assert.Equal(1, t[[0, 0]]);
        Assert.Equal(2, t[[0, 1]]);
        Assert.Equal(3, t[[1, 0]]);
        Assert.Equal(4, t[[1, 1]]);
    }

    [Fact]
    public void Create_3D_Array()
    {
        var data = new int[,,]
        {
            { { 1, 2, 3 }, { 4,   5,  6 } },
            { { 7, 8, 9 }, { 10, 11, 12 } }
        };

        var t = MemoryND.Create(data);

        Assert.Equal(3, t.Shape.Length);
        Assert.Equal(12u, t.Length);
        Assert.Equal(1, t[[0, 0, 0]]);
        Assert.Equal(2, t[[0, 0, 1]]);
        Assert.Equal(4, t[[0, 1, 0]]);
        Assert.Equal(7, t[[1, 0, 0]]);
        Assert.Equal(8, t[[1, 0, 1]]);
        Assert.Equal(10, t[[1, 1, 0]]);
        Assert.Equal(12, t[[1, 1, 2]]);
    }

    [Fact]
    public void Create_4D_Array()
    {
        var data = new int[,,,]
        {
            {
                { { 1, 2 }, { 3, 4 } },
                { { 5, 6 }, { 7, 8 } }
            },
            {
                { { 9, 10 }, { 11, 12 } },
                { { 13, 14 }, { 15, 16 } }
            }
        };

        var t = MemoryND.Create(data);

        Assert.Equal(4, t.Shape.Length);
        Assert.Equal(16u, t.Length);
        Assert.Equal(1, t[[0, 0, 0, 0]]);
        Assert.Equal(2, t[[0, 0, 0, 1]]);
        Assert.Equal(3, t[[0, 0, 1, 0]]);
        Assert.Equal(5, t[[0, 1, 0, 0]]);
        Assert.Equal(6, t[[0, 1, 0, 1]]);
        Assert.Equal(9, t[[1, 0, 0, 0]]);
        Assert.Equal(10, t[[1, 0, 0, 1]]);
        Assert.Equal(13, t[[1, 1, 0, 0]]);
        Assert.Equal(16, t[[1, 1, 1, 1]]);

    }

    [Fact]
    public void Create_2D_Memory2D()
    {
        Memory2D<int> data = new int[,] { { 1, 2 }, { 3, 4 } };

        var t = new MemoryND<int>(data);

        Assert.Equal(2, t.Shape.Length);
        Assert.Equal(4u, t.Length);
        Assert.Equal(1, t[[0, 0]]);
        Assert.Equal(2, t[[0, 1]]);
        Assert.Equal(3, t[[1, 0]]);
        Assert.Equal(4, t[[1, 1]]);
    }

    [Fact]
    public void CreateWithDimensions_Memory()
    {
        var t = MemoryND.CreateWithDimensions<int>([2, 2, 2]);
        Assert.Equal(3, t.Shape.Length);
        Assert.Equal(8u, t.Length);
    }

    [Fact]
    public void CreateWithDimensions_Tuple_2()
    {
        var t = MemoryND.CreateWithDimensions<int>(2, 2);
        Assert.Equal(2, t.Shape.Length);
        Assert.Equal(4u, t.Length);
    }

    [Fact]
    public void CreateWithDimensions_Tuple_3()
    {
        var t = MemoryND.CreateWithDimensions<int>(2, 2, 2);
        Assert.Equal(3, t.Shape.Length);
        Assert.Equal(8u, t.Length);
    }
}
