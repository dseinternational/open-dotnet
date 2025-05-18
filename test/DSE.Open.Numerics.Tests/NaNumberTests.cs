// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

public class NaNumberTests
{
    [Fact]
    public void Add()
    {
        NaNumber<int>[] sequence1 = [1, 2, 3, 4, 5];
        NaNumber<int>[] sequence2 = [1, 2, 3, 4, 5];
        var dest = new NaNumber<int>[5];
        NaNumberPrimitives.Add(sequence1, sequence2, dest);

        Assert.Equal([2, 4, 6, 8, 10], dest);
    }

    [Fact]
    public void Sum()
    {
        NaNumber<int>[] sequence = [1, 2, 3, 4, 5];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Na()
    {
        NaNumber<int>[] sequence = [1, 2, 3, NaNumber<int>.Na, 4, 5, NaNumber<int>.Na, 6, 7, 8];
        var sum = NaNumberPrimitives.Sum(sequence);
        Assert.Equal("NA", sum.ToString());
    }
}

public static class NaNumberPrimitives
{
    public static void Add<T>(ReadOnlySpan<NaNumber<T>> x, ReadOnlySpan<NaNumber<T>> y, Span<NaNumber<T>> destination)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);

        var xv = MemoryMarshal.Cast<NaNumber<T>, T>(x);
        var yv = MemoryMarshal.Cast<NaNumber<T>, T>(y);
        var dv = MemoryMarshal.Cast<NaNumber<T>, T>(destination);

        TensorPrimitives.Add(xv, yv, dv);

        if (xv.Contains(NaNumber<T>.Sentinel) || yv.Contains(NaNumber<T>.Sentinel))
        {
            for (var i = 0; i < dv.Length; i++)
            {
                if (xv[i] == NaNumber<T>.Sentinel || yv[i] == NaNumber<T>.Sentinel)
                {
                    dv[i] = NaNumber<T>.Sentinel;
                }
            }
        }
    }

    public static NaNumber<T> Sum<T>(ReadOnlySpan<NaNumber<T>> sequence)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        if (sequence.IsEmpty)
        {
            return T.Zero;
        }

        if (sequence.Length == 1)
        {
            return sequence[0];
        }

        var values = MemoryMarshal.Cast<NaNumber<T>, T>(sequence);

        if (values.Contains(NaNumber<T>.Sentinel))
        {
            return NaNumber<T>.Na;
        }

        return TensorPrimitives.Sum(values);
    }
}
