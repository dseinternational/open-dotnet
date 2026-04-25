// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// </summary>
/// <remarks>
/// Type-erased base for <see cref="ReadOnlyVector{T}"/>. Most callers should
/// work with the generic form; the non-generic base exists primarily for
/// serialization and runtime dispatch through <see cref="VectorDataType"/>.
/// </remarks>
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlyVector : VectorBase, IReadOnlyVector
{
    /// <summary>
    /// Initializes the read-only vector base with the supplied dtype, runtime
    /// element type, and length. Used by derived <see cref="ReadOnlyVector{T}"/>.
    /// </summary>
    protected internal ReadOnlyVector(VectorDataType dataType, Type itemType, int length)
        : base(dataType, itemType, length, true)
    {
    }

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/>.
    /// </summary>
    /// <param name="index">Zero-based index of the element to read.</param>
    public abstract VectorValue GetVectorValue(int index);

    /// <summary>
    /// Creates a read-only vector that wraps <paramref name="array"/> by
    /// reference. The caller is responsible for ensuring the array is not
    /// mutated while the vector is in use; otherwise the "read-only" guarantee
    /// is violated.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
    public static ReadOnlyVector<T> Create<T>(T[] array)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Length == 0)
        {
            return ReadOnlyVector<T>.Empty;
        }

        return new ReadOnlyVector<T>(array);
    }

    /// <summary>
    /// Creates a read-only vector that wraps <paramref name="memory"/> by
    /// reference; the vector and the memory share storage.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public static ReadOnlyVector<T> Create<T>(ReadOnlyMemory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return ReadOnlyVector<T>.Empty;
        }

        return new ReadOnlyVector<T>(memory);
    }

    /// <summary>
    /// Creates a read-only vector by copying the contents of <paramref name="span"/>
    /// into a freshly-allocated array.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public static ReadOnlyVector<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
            return ReadOnlyVector<T>.Empty;
        }

        return new ReadOnlyVector<T>(span.ToArray().AsMemory());
    }
}
