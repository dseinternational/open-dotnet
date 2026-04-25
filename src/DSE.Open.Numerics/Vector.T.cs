// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The element type. Must implement <see cref="IEquatable{T}"/>.</typeparam>
[CollectionBuilder(typeof(Vector), nameof(Create))]
[JsonConverter(typeof(VectorJsonConverter))]
public sealed class Vector<T>
    : Vector,
      IVector<T>,
      IReadOnlyVector<T>,
      IEquatable<Vector<T>>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets an empty vector.
    /// </summary>
    public static readonly Vector<T> Empty = new(Memory<T>.Empty);

    private readonly Memory<T> _memory;

    /// <summary>
    /// Creates a new vector of the given <paramref name="length"/> backed by a
    /// freshly-allocated <typeparamref name="T"/>[] with all elements at
    /// <c>default(T)</c>.
    /// </summary>
    public Vector(int length) : this(new T[length])
    {
    }

    /// <summary>
    /// Creates a new vector that wraps <paramref name="array"/> by reference;
    /// no copy is made and writes through the vector are visible to the array
    /// (and vice versa).
    /// </summary>
    public Vector(T[] array) : this(array.AsMemory())
    {
    }

    /// <summary>
    /// Creates a new vector that wraps <paramref name="memory"/> by reference;
    /// the vector and the underlying memory share storage.
    /// </summary>
    public Vector(Memory<T> memory) : base(GetDataType<T>(), typeof(T), memory.Length)
    {
        _memory = memory;
    }

    int IReadOnlyCollection<T>.Count => Length;

    /// <summary>
    /// Gets or sets the element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Zero-based index of the element.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when
    /// <paramref name="index"/> is outside <c>[0, Length)</c>.</exception>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory.Span[index] = value;
    }

    /// <inheritdoc />
    public override VectorValue GetVectorValue(int index)
    {
        return VectorValue.FromValue(this[index]);
    }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return _memory.Span;
    }

    ReadOnlySpan<T> IReadOnlyVector<T>.AsSpan()
    {
        return AsSpan();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    /// <summary>
    /// Returns a hash code for this vector. Two vectors that compare equal via
    /// <see cref="Equals(Vector{T})"/> produce the same hash code; the hash is
    /// derived from the entire element sequence and is therefore O(N).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        if (_memory.IsEmpty)
        {
            return 0;
        }

        switch (_memory)
        {
            case Memory<byte> memory:
                return GetHashCode(memory);
            case Memory<sbyte> memory:
                return GetHashCode(memory);
            case Memory<short> memory:
                return GetHashCode(memory);
            case Memory<ushort> memory:
                return GetHashCode(memory);
            case Memory<int> memory:
                return GetHashCode(memory);
            case Memory<uint> memory:
                return GetHashCode(memory);
            case Memory<long> memory:
                return GetHashCode(memory);
            case Memory<ulong> memory:
                return GetHashCode(memory);
            case Memory<float> memory:
                return GetHashCode(memory);
            case Memory<double> memory:
                return GetHashCode(memory);
            case Memory<Half> memory:
                return GetHashCode(memory);
            case Memory<decimal> memory:
                return GetHashCode(memory);
            default:
                break;
        }

        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    private static int GetHashCode<TMem>(Memory<TMem> memory)
        where TMem : struct
    {
        var hash = new HashCode();
        hash.AddBytes(MemoryMarshal.AsBytes(memory.Span));
        return hash.ToHashCode();
    }

    /// <summary>
    /// Returns a read-only view of this vector. The view shares the underlying
    /// memory; mutations made through the source vector are observable.
    /// </summary>
    public new ReadOnlyVector<T> AsReadOnly()
    {
        return new ReadOnlyVector<T>(_memory);
    }

    /// <inheritdoc />
    protected override ReadOnlyVector CreateReadOnly()
    {
        return AsReadOnly();
    }

    /// <summary>
    /// Compares the elements of this vector with the elements of another using the equality operator
    /// provided by the implementation of <see cref="IEqualityOperators{TSelf,TOther,Boolean}"/>
    /// offered by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Vector<T>? other)
    {
        return other is not null && VectorPrimitives.SequenceEqual(this, other);
    }

    /// <summary>
    /// Compares the elements of this vector with the elements of another using the equality operator
    /// provided by the implementation of <see cref="IEqualityOperators{TSelf,TOther,Boolean}"/>
    /// offered by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && VectorPrimitives.SequenceEqual(this, other);
    }

    /// <summary>
    /// Compares the elements of this vector with the elements of another using the equality operator
    /// provided by the implementation of <see cref="IEqualityOperators{TSelf,TOther,Boolean}"/>
    /// offered by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ReadOnlySpan<T> other)
    {
        return VectorPrimitives.SequenceEqual(this, other);
    }

    /// <summary>
    /// Returns a struct enumerator over the elements of the vector. <c>foreach</c>
    /// resolves to this overload via duck-typing and avoids the per-iteration
    /// heap allocation of an interface-based enumerator.
    /// </summary>
    public MemoryEnumerator<T> GetEnumerator()
    {
        return _memory.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable((ReadOnlyMemory<T>)_memory).GetEnumerator();
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
    public Vector<T> Slice(int start)
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
    public Vector<T> Slice(int start, int length)
    {
        return _memory.Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IReadOnlyVector<T> IReadOnlyVector<T>.Slice(int start, int length)
    {
        return Slice(start, length).AsReadOnly();
    }

    IVector<T> IVector<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    /// <summary>
    /// Copies the contents of this vector to a new array.
    /// </summary>
    /// <returns></returns>
    public T[] ToArray()
    {
        return _memory.ToArray();
    }

    /// <summary>
    /// Wraps a <see cref="Memory{T}"/> as a <see cref="Vector{T}"/> by reference;
    /// no copy is made.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    /// <summary>
    /// Copies the contents of <paramref name="vector"/> into a new
    /// <see cref="Vector{T}"/>. To wrap an array by reference, use
    /// <see cref="Vector.Create{T}(T[])"/> instead.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return [.. vector];
    }

    /// <summary>
    /// Returns the underlying <see cref="Memory{T}"/> of <paramref name="vector"/>,
    /// or <c>default</c> when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Memory<T>(Vector<T>? vector)
    {
        return vector is not null ? vector._memory : default;
    }

    /// <summary>
    /// Returns a read-only view of <paramref name="vector"/> via
    /// <see cref="AsReadOnly"/>, or the shared empty
    /// <see cref="ReadOnlyVector{T}"/> when <paramref name="vector"/> is
    /// <see langword="null"/>.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
