// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

public static class NaNumberPrimitives
{
    public static void Add<T>(ReadOnlySpan<NaInt<T>> x, ReadOnlySpan<NaInt<T>> y, Span<NaInt<T>> destination)
        where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
    {
        NumericsArgumentException.ThrowIfNotEqualLength(x, y, destination);

        var xv = MemoryMarshal.Cast<NaInt<T>, T>(x);
        var yv = MemoryMarshal.Cast<NaInt<T>, T>(y);
        var dv = MemoryMarshal.Cast<NaInt<T>, T>(destination);

        TensorPrimitives.Add(xv, yv, dv);

        if (xv.Contains(NaInt<T>.Sentinel) || yv.Contains(NaInt<T>.Sentinel))
        {
            for (var i = 0; i < dv.Length; i++)
            {
                if (xv[i] == NaInt<T>.Sentinel || yv[i] == NaInt<T>.Sentinel)
                {
                    dv[i] = NaInt<T>.Sentinel;
                }
            }
        }
    }

    public static NaInt<T> Sum<T>(ReadOnlySpan<NaInt<T>> sequence)
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

        var values = MemoryMarshal.Cast<NaInt<T>, T>(sequence);

        if (values.Contains(NaInt<T>.Sentinel))
        {
            return NaInt<T>.Na;
        }

        return TensorPrimitives.Sum(values);
    }
}
