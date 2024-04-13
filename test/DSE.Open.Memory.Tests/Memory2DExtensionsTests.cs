// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Tests.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public class Memory2DExtensionsTests
{
    [Fact]
    public void Contains()
    {
        var m = new Memory2D<int>(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });

        for (var i = 1; i < 7; i++)
        {
            Assert.True(m.Contains(i));
        }

        for (var i = 7; i < 20; i++)
        {
            Assert.False(m.Contains(i));
        }

        for (var i = 0; i > -20; i--)
        {
            Assert.False(m.Contains(i));
        }
    }

    [Fact]
    public void Contains_ReadOnly()
    {
        var m = new ReadOnlyMemory2D<int>(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });

        for (var i = 1; i < 7; i++)
        {
            Assert.True(m.Contains(i));
        }

        for (var i = 7; i < 20; i++)
        {
            Assert.False(m.Contains(i));
        }

        for (var i = 0; i > -20; i--)
        {
            Assert.False(m.Contains(i));
        }
    }

    [Fact]
    public void ContainsAny()
    {
        var m = new Memory2D<int>(new int[,] { { 1, 2, 3 }, { 3, 4, 5 }, { 5, 6, 7 } });

        for (var i = 1; i < 8; i++)
        {
            Assert.True(m.ContainsAny([i]));
        }

        for (var i = 1; i < 8; i++)
        {
            Assert.True(m.ContainsAny([10, 11, i]));
        }

        for (var i = 1; i < 8; i++)
        {
            Assert.False(m.ContainsAny([10, 11, i + 20]));
        }
    }

    [Fact]
    public void ContainsAny_ReadOnly()
    {
        var m = new ReadOnlyMemory2D<int>(new int[,] { { 1, 2, 3 }, { 3, 4, 5 }, { 5, 6, 7 } });

        for (var i = 1; i < 8; i++)
        {
            Assert.True(m.ContainsAny([i]));
        }

        for (var i = 1; i < 8; i++)
        {
            Assert.True(m.ContainsAny([10, 11, i]));
        }

        for (var i = 1; i < 8; i++)
        {
            Assert.False(m.ContainsAny([10, 11, i + 20]));
        }
    }

    [Fact]
    public void ContainsAny_SearchValues()
    {
        var m = new Memory2D<char>(new char[,] { { 'a', 'b', 'c' }, { 'c', 'd', 'e' }, { 'e', 'f', 'g' } });

        _ = m.TryGetMemory(out var memory);

        foreach (var c in memory)
        {
            Assert.True(m.ContainsAny(SearchValues.Create(new char[] { 'z', 'y', 'x', c })));
        }
    }

    [Fact]
    public void IndexOf()
    {
        var m = new Memory2D<int>(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });
        Assert.Equal(new Index2D(1, 1), m.IndexOf(4));
        Assert.Equal(new Index2D(2, 1), m.IndexOf(6));
    }

    [Fact]
    public void IndexOf_ReadOnly()
    {
        var m = new ReadOnlyMemory2D<int>(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });
        Assert.Equal(new Index2D(1, 1), m.IndexOf(4));
        Assert.Equal(new Index2D(2, 1), m.IndexOf(6));
    }
}
