// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector<T> ToVector<T>(this Memory<T> memory)
    {
        return memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyVector<T> ToReadOnlyVector<T>(this ReadOnlyMemory<T> memory)
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector<T> ToVector<T>(this Span<T> span)
    {
        return span.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyVector<T> ToReadOnlyVector<T>(this ReadOnlySpan<T> span)
    {
        if (span.IsEmpty)
        {
            return [];
        }

        return span.ToArray();
    }
}
