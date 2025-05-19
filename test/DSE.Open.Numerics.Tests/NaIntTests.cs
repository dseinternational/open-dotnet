// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

public class NaIntTests
{
    [Fact]
    public void Add()
    {
        NaInt<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInt<int>[] sequence2 = [1, 2, 3, 4, 5];
        var dest = new NaInt<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, 10], dest);
    }

    [Fact]
    public void Add_Na()
    {
        NaInt<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInt<int>[] sequence2 = [1, 2, 3, 4, NaInt<int>.Na];
        var dest = new NaInt<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, NaInt<int>.Sentinel], MemoryMarshal.Cast<NaInt<int>, int>(dest));
    }

    [Fact]
    public void Sum()
    {
        NaInt<int>[] sequence = [1, 2, 3, 4, 5];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Na()
    {
        NaInt<int>[] sequence = [1, 2, 3, NaInt<int>.Na, 4, 5, null, 6, 7, 8];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal("NA", sum.ToString());
    }
}
