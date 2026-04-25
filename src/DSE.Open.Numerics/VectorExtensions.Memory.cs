// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    /// <summary>
    /// Wraps <paramref name="memory"/> as a <see cref="Vector{T}"/> by reference;
    /// no copy is made.
    /// </summary>
    public static Vector<T> ToVector<T>(this Memory<T> memory)
        where T : IEquatable<T>
    {
        return memory;
    }

    /// <summary>
    /// Wraps <paramref name="memory"/> as a <see cref="ReadOnlyVector{T}"/> by
    /// reference; no copy is made. Returns the shared empty vector when
    /// <paramref name="memory"/> is empty.
    /// </summary>
    public static ReadOnlyVector<T> ToReadOnlyVector<T>(this ReadOnlyMemory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return memory;
    }

    /// <summary>
    /// Copies the contents of <paramref name="span"/> into a freshly-allocated
    /// <see cref="Vector{T}"/>.
    /// </summary>
    public static Vector<T> ToVector<T>(this Span<T> span)
        where T : IEquatable<T>
    {
        return span.ToArray();
    }

    /// <summary>
    /// Copies the contents of <paramref name="span"/> into a freshly-allocated
    /// <see cref="ReadOnlyVector{T}"/>. Returns the shared empty vector when
    /// <paramref name="span"/> is empty.
    /// </summary>
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
