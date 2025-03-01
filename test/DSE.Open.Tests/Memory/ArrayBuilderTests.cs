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
        using var builder = new ArrayBuilder<int>([1, 2, 3, 4, 5, 6]);
        Assert.Equal(6, builder.Count);
        Assert.False(builder.BufferAllocated);

        for (var i = 0; i < builder.Count; i++)
        {
            Assert.Equal(i + 1, builder[i]);
        }

        builder.Add(7);

        Assert.Equal(7, builder.Count);

        var array = builder.ToArray();

        Assert.Equal(7, array.Length);
    }
}
