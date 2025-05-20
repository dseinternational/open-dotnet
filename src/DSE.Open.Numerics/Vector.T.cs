// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
public sealed class Vector<T> : Vector, IVector<T>, IReadOnlyVector<T>, IEquatable<Vector<T>>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets an empty vector.
    /// </summary>
    public static readonly Vector<T> Empty = new(Memory<T>.Empty);

    private readonly Memory<T> _memory;

    public Vector(T[] array) : this(array.AsMemory())
    {
    }

    public Vector(Memory<T> memory)
        : base(Vector.GetDataType<T>(), typeof(T), memory.Length)
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
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

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

    public bool Equals(Vector<T>? other)
    {
        return other is not null && SequenceEqual(this, other);
    }

    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && SequenceEqual(this, other);
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return SequenceEqual(this, other);
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
