// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

public class NaIntegerTests
{
    [Fact]
    public void Add()
    {
        NaInteger<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInteger<int>[] sequence2 = [1, 2, 3, 4, 5];
        var dest = new NaInteger<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, 10], dest);
    }

    [Fact]
    public void Add_Na()
    {
        NaInteger<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaInteger<int>[] sequence2 = [1, 2, 3, 4, NaInteger<int>.Na];
        var dest = new NaInteger<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, NaInteger<int>.Sentinel], MemoryMarshal.Cast<NaInteger<int>, int>(dest));
    }

    [Fact]
    public void Sum()
    {
        NaInteger<int>[] sequence = [1, 2, 3, 4, 5];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Na()
    {
        NaInteger<int>[] sequence = [1, 2, 3, NaInteger<int>.Na, 4, 5, null, 6, 7, 8];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal("NA", sum.ToString());
    }
}

public static class NaNumberPrimitives
{
    public static void Add<T>(ReadOnlySpan<NaInteger<T>> x, ReadOnlySpan<NaInteger<T>> y, Span<NaInteger<T>> destination)
        where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);

        var xv = MemoryMarshal.Cast<NaInteger<T>, T>(x);
        var yv = MemoryMarshal.Cast<NaInteger<T>, T>(y);
        var dv = MemoryMarshal.Cast<NaInteger<T>, T>(destination);

        TensorPrimitives.Add(xv, yv, dv);

        if (xv.Contains(NaInteger<T>.Sentinel) || yv.Contains(NaInteger<T>.Sentinel))
        {
            for (var i = 0; i < dv.Length; i++)
            {
                if (xv[i] == NaInteger<T>.Sentinel || yv[i] == NaInteger<T>.Sentinel)
                {
                    dv[i] = NaInteger<T>.Sentinel;
                }
            }
        }
    }

    public static NaInteger<T> Sum<T>(ReadOnlySpan<NaInteger<T>> sequence)
        where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
    {
        if (sequence.IsEmpty)
        {
            return T.Zero;
        }

        if (sequence.Length == 1)
        {
            return sequence[0];
        }

        var values = MemoryMarshal.Cast<NaInteger<T>, T>(sequence);

        if (values.Contains(NaInteger<T>.Sentinel))
        {
            return NaInteger<T>.Na;
        }

        return TensorPrimitives.Sum(values);
    }
}
