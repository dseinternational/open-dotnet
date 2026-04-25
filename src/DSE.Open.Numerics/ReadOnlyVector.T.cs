// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values of data type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
[CollectionBuilder(typeof(ReadOnlyVector), nameof(Create))]
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public sealed class ReadOnlyVector<T> : ReadOnlyVector, IReadOnlyVector<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// The shared empty <see cref="ReadOnlyVector{T}"/>.
    /// </summary>
    public static readonly ReadOnlyVector<T> Empty = new(Memory<T>.Empty);

    private readonly ReadOnlyMemory<T> _memory;

    /// <summary>
    /// Creates a read-only vector that wraps <paramref name="array"/> by
    /// reference. The caller must not mutate the array while the vector is in
    /// use.
    /// </summary>
    public ReadOnlyVector(T[] array) : this(array.AsMemory())
    {
    }

    /// <summary>
    /// Creates a read-only vector that wraps <paramref name="memory"/> by
    /// reference; the vector and the memory share storage.
    /// </summary>
    public ReadOnlyVector(ReadOnlyMemory<T> memory)
        : base(Vector.GetDataType<T>(), typeof(T), memory.Length)
    {
        _memory = memory;
    }

    int IReadOnlyCollection<T>.Count => Length;

    /// <summary>
    /// Gets the element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Zero-based index of the element.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when
    /// <paramref name="index"/> is outside <c>[0, Length)</c>.</exception>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory.Span[index];
    }

    /// <inheritdoc />
    public override VectorValue GetVectorValue(int index)
    {
        return VectorValue.FromValue(this[index]);
    }

    /// <summary>
    /// Gets a read-only span over the contents of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsSpan()
    {
        return _memory.Span;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    /// <summary>
    /// Returns a hash code for this vector. The hash is derived from the entire
    /// element sequence and is therefore O(N) in <see cref="VectorBase.Length"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="other"/> is non-null
    /// and contains the same sequence of elements as this vector.
    /// </summary>
    public bool Equals(ReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="other"/> is non-null
    /// and contains the same sequence of elements as this vector.
    /// </summary>
    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="other"/> contains
    /// the same sequence of elements as this vector.
    /// </summary>
    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsSpan().SequenceEqual(other);
    }

    /// <summary>
    /// Returns a struct enumerator over the elements of the vector. <c>foreach</c>
    /// resolves to this overload via duck-typing and avoids the per-iteration
    /// heap allocation of an interface-based enumerator.
    /// </summary>
    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return _memory.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable(_memory).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    /// <summary>
    /// Returns a slice of this vector starting at <paramref name="start"/>.
    /// The slice shares the underlying storage; no copy is made.
    /// </summary>
    /// <param name="start">Zero-based start index of the slice.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyVector<T> Slice(int start)
    {
        return _memory[start..];
    }

    /// <summary>
    /// Returns a slice of this vector. The slice shares the underlying storage;
    /// no copy is made.
    /// </summary>
    /// <param name="start">Zero-based start index of the slice.</param>
    /// <param name="length">Number of elements in the slice.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyVector<T> Slice(int start, int length)
    {
        return _memory.Slice(start, length);
    }

    IReadOnlyVector<T> IReadOnlyVector<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="left"/> is non-null
    /// and equals <paramref name="right"/> by value (sequence equality).
    /// <see langword="null"/> on the left always compares unequal.
    /// </summary>
    public static bool operator ==(ReadOnlyVector<T>? left, ReadOnlyVector<T>? right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    /// <summary>
    /// Negation of <see cref="op_Equality(ReadOnlyVector{T}?, ReadOnlyVector{T}?)"/>.
    /// </summary>
    public static bool operator !=(ReadOnlyVector<T>? left, ReadOnlyVector<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Creates a mutable <see cref="Vector{T}"/> from this read-only vector.
    /// </summary>
    public Vector<T> ToVector()
    {
        return new Vector<T>(_memory.ToArray());
    }

    /// <summary>
    /// Wraps a <see cref="ReadOnlyMemory{T}"/> as a <see cref="ReadOnlyVector{T}"/>
    /// by reference; no copy is made.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
    }

    /// <summary>
    /// Wraps an array as a <see cref="ReadOnlyVector{T}"/> by reference. The
    /// caller must not mutate the array while the vector is in use.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector);
    }

    /// <summary>
    /// Returns the underlying <see cref="ReadOnlyMemory{T}"/> of <paramref name="vector"/>,
    /// or <c>default</c> when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlyVector<T> vector)
    {
        return vector is not null ? vector._memory : default;
    }
}
