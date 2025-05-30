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
/// <typeparam name="T"></typeparam>
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

    public Vector(int length) : this(new T[length])
    {
    }

    public Vector(T[] array) : this(array.AsMemory())
    {
    }

    public Vector(Memory<T> memory) : base(GetDataType<T>(), typeof(T), memory.Length)
    {
        _memory = memory;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory.Span[index] = value;
    }

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

#pragma warning disable CA1033 // Interface methods should be callable by child types
    ReadOnlySpan<T> IReadOnlyVector<T>.AsSpan()
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return AsSpan();
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

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

    public new ReadOnlyVector<T> AsReadOnly()
    {
        return new ReadOnlyVector<T>(_memory);
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector<T> Slice(int start)
    {
        return _memory[start..];
    }

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

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return [.. vector];
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Memory<T>(Vector<T>? vector)
    {
        return vector is not null ? vector._memory : default;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
