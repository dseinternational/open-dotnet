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
/// A serializable, contiguous sequence of values of known length and data type.
/// </summary>
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlyVector : VectorBase, IReadOnlyVector
{
    protected internal ReadOnlyVector(VectorDataType dataType, Type itemType, int length)
        : base(dataType, itemType, length, true)
    {
    }

    public static ReadOnlyVector<T> Create<T>(ReadOnlySpan<T> span)
    {
        if (span.IsEmpty)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlyVector<T>(span.ToArray().AsMemory());
    }
}

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values of data type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(ReadOnlyVector), nameof(Create))]
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public sealed class ReadOnlyVector<T> : ReadOnlyVector, IReadOnlyVector<T>
{
    public static readonly ReadOnlyVector<T> Empty = new(Memory<T>.Empty);

    private readonly ReadOnlyMemory<T> _memory;

    public ReadOnlyVector(T[] array) : this(array.AsMemory())
    {
    }

    public ReadOnlyVector(ReadOnlyMemory<T> memory)
        : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), memory.Length)
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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsSpan()
    {
        return _memory.Span;
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

    public bool Equals(ReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsSpan().SequenceEqual(other);
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyVector<T> Slice(int start)
    {
        return _memory[start..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyVector<T> Slice(int start, int length)
    {
        return _memory.Slice(start, length);
    }

    IReadOnlyVector<T> IReadOnlyVector<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    public static bool operator ==(ReadOnlyVector<T>? left, ReadOnlyVector<T>? right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    public static bool operator !=(ReadOnlyVector<T>? left, ReadOnlyVector<T>? right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlyVector<T> vector)
    {
        return vector is not null ? vector._memory : default;
    }
}
