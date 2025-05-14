// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Series<T> ToVector<T>(this Memory<T> memory)
        where T : IEquatable<T>
    {
        return memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySeries<T> ToReadOnlyVector<T>(this ReadOnlyMemory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Series<T> ToVector<T>(this Span<T> span)
        where T : IEquatable<T>
    {
        return span.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySeries<T> ToReadOnlyVector<T>(this ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
            return [];
        }

        return span.ToArray();
    }
}
