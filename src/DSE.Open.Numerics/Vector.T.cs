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
/// A serializable sequence of values of known length of type <typeparamref name="T"/> 
/// with value equality semantics.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(Vector), nameof(Create))]
[JsonConverter(typeof(VectorJsonConverter))]
public class Vector<T> : Vector, IVector<T>, IReadOnlyVector<T>
{
    public static readonly Vector<T> Empty = new([]);

    internal Vector(T[] data) : this(new Memory<T>(data))
    {
    }

    internal Vector(T[] data, int start, int length) : this(new Memory<T>(data, start, length))
    {
    }

    internal Vector(Memory<T> data) : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), data.Length)
    {
        Data = data;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Data.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Data.Span[index] = value;
    }

    protected Memory<T> Data { get; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Data.Span;
    }

    ReadOnlySpan<T> IReadOnlyVector<T>.Span => Span;

    public ReadOnlyVector<T> AsReadOnly()
    {
        return new((ReadOnlyMemory<T>)Data);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    public MemoryEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    ReadOnlyMemoryEnumerator<T> IReadOnlyVector<T>.GetEnumerator()
    {
        return ((ReadOnlyMemory<T>)Data).GetEnumerator();
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

    public bool Equals(Vector<T>? other)
    {
        return other is not null && Equals(other.Span);
    }

    public bool Equals(IVector<T>? other)
    {
        return other is not null && Equals(other.Span);
    }

    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.Span);
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return Span.SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return Data.ToArray();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable<T>(Data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    public static bool operator ==(Vector<T>? left, Vector<T>? right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    public static bool operator !=(Vector<T>? left, Vector<T>? right)
    {
        return !(left == right);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Vector<T>(T[] vector)
    {
        return new(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Vector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Memory<T>(Vector<T> vector)
    {
        return vector is not null ? vector.Data : default;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
