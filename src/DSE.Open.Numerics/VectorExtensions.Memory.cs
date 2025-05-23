// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    public static Vector<T> ToVector<T>(this Memory<T> memory)
        where T : IEquatable<T>
    {
        return memory;
    }

    public static ReadOnlyVector<T> ToReadOnlyVector<T>(this ReadOnlyMemory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return memory;
    }

    public static Vector<T> ToVector<T>(this Span<T> span)
        where T : IEquatable<T>
    {
        return span.ToArray();
    }

    public static ReadOnlyVector<T> ToReadOnlyVector<T>(this ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
            return [];
        }

        return span.ToArray();
    }
}
