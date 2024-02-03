// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Add_Int32_Array_Int32()
    {
        int[] s1 = [1, 2, 3, 4, 5];
        int[] s2 = [1, 2, 3, 4, 5];
        var sum = new int[5];

        Vector.Add(s1, s2, sum);

        Assert.Equal(2, sum[0]);
        Assert.Equal(4, sum[1]);
        Assert.Equal(6, sum[2]);
        Assert.Equal(8, sum[3]);
        Assert.Equal(10, sum[4]);
    }

    [Fact]
    public void Add_Int32_Span_Int32()
    {
        Span<int> s1 = [1, 2, 3, 4, 5];
        Span<int> s2 = [1, 2, 3, 4, 5];
        Span<int> sum = stackalloc int[5];

        Vector.Add(s1, s2, sum);

        Assert.Equal(2, sum[0]);
        Assert.Equal(4, sum[1]);
        Assert.Equal(6, sum[2]);
        Assert.Equal(8, sum[3]);
        Assert.Equal(10, sum[4]);
    }

    [Fact]
    public void Add_Int32_ImmutableArray_Span_Int32()
    {
        ImmutableArray<int> s1 = [1, 2, 3, 4, 5];
        ImmutableArray<int> s2 = [1, 2, 3, 4, 5];
        Span<int> sum = stackalloc int[5];

        Vector.Add(s1, s2, sum);

        Assert.Equal(2, sum[0]);
        Assert.Equal(4, sum[1]);
        Assert.Equal(6, sum[2]);
        Assert.Equal(8, sum[3]);
        Assert.Equal(10, sum[4]);
    }

    [Fact]
    public void Add_Int32_ImmutableArray_Array_Int32()
    {
        ImmutableArray<int> s1 = [1, 2, 3, 4, 5];
        ImmutableArray<int> s2 = [1, 2, 3, 4, 5];
        var sum = new int[5];

        Vector.Add(s1, s2, sum);

        Assert.Equal(2, sum[0]);
        Assert.Equal(4, sum[1]);
        Assert.Equal(6, sum[2]);
        Assert.Equal(8, sum[3]);
        Assert.Equal(10, sum[4]);
    }

    [Fact]
    public void AddInPace_Int32_Zeroes_Ones()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        Vector.AddInPace(v1, v2);
        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        Assert.True(v1.SequenceEqual(v2));
    }
}
