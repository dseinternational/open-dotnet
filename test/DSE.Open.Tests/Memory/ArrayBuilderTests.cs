// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Memory;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public class ArrayBuilderTests
{
    [Fact]
    public void InitialiseOwned()
    {
        using var builder = new ArrayBuilder<int>();
        Assert.Equal(0, builder.Count);
        Assert.Equal(ArrayBuilder.DefaultOwnedCapacity, builder.Capacity);
    }

    [Fact]
    public void InitialiseWithBuffer()
    {
        Span<int> buffer = stackalloc int[6];

        using var builder = new ArrayBuilder<int>(buffer);

        Assert.Equal(0, builder.Count);
        Assert.Equal(6, builder.Capacity);
    }

    [Fact]
    public void EnsureCapacityGrowsPooledBufferToAccommodateItems()
    {
        using var builder = new ArrayBuilder<int>(capacity: 4, rentFromPool: true);

        const int ItemCount = 2048;

        for (var i = 0; i < ItemCount; i++)
        {
            builder.Add(i);
        }

        Assert.Equal(ItemCount, builder.Count);
        Assert.True(builder.Capacity >= ItemCount);

        var result = builder.ToArray();
        for (var i = 0; i < ItemCount; i++)
        {
            Assert.Equal(i, result[i]);
        }
    }

    [Fact]
    public void EnsureCapacityGrowsOwnedBufferToAccommodateItems()
    {
        using var builder = new ArrayBuilder<int>(capacity: 4, rentFromPool: false);

        const int ItemCount = 2048;

        for (var i = 0; i < ItemCount; i++)
        {
            builder.Add(i);
        }

        Assert.Equal(ItemCount, builder.Count);
        Assert.True(builder.Capacity >= ItemCount);
    }
}
