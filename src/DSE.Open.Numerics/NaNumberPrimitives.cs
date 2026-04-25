// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

/// <summary>
/// SIMD-friendly element-wise primitives over <see cref="NaInt{T}"/> spans. Operations
/// reinterpret the spans as raw underlying values via
/// <see cref="MemoryMarshal.Cast{TFrom, TTo}(ReadOnlySpan{TFrom})"/> so that the
/// underlying <see cref="System.Numerics.Tensors.TensorPrimitives"/> kernels run
/// unchanged, then re-mask any positions whose inputs were the NA sentinel
/// (<see cref="NaInt{T}.Sentinel"/>).
/// </summary>
public static class NaNumberPrimitives
{
    /// <summary>
    /// Computes element-wise <paramref name="x"/> + <paramref name="y"/> into
    /// <paramref name="destination"/>. Positions where either input is NA produce NA.
    /// </summary>
    /// <typeparam name="T">The underlying integer type.</typeparam>
    /// <exception cref="NumericsArgumentException">The three spans don't share the same length.</exception>
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

    /// <summary>
    /// Returns the sum of <paramref name="sequence"/>. If any element is NA, the
    /// result is NA; if the sequence is empty, the result is zero.
    /// </summary>
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
