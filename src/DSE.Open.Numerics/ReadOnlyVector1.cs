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
[CollectionBuilder(typeof(ReadOnlyVector), nameof(Create))]
[JsonConverter(typeof(VectorJsonConverter))]
public class ReadOnlyVector<T> : ReadOnlyVector, IReadOnlyVector<T>
{
    public static readonly ReadOnlyVector<T> Empty = new([]);

    internal ReadOnlyVector(T[] data) : this(new ReadOnlyMemory<T>(data))
    {
    }

    internal ReadOnlyVector(T[] data, int start, int length) : this(new ReadOnlyMemory<T>(data, start, length))
    {
    }

    internal ReadOnlyVector(ReadOnlyMemory<T> data) : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), data.Length)
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
    }

    protected ReadOnlyMemory<T> Data { get; }

    ReadOnlyMemory<T> IReadOnlyVector<T>.Data => Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return Data.Span;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
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
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsReadOnlySpan().SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return Data.ToArray();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable(Data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> Slice(int start, int length)
    {
        return AsReadOnlySpan().Slice(start, length);
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
    public static implicit operator ReadOnlyVector<T>(T[] vector)
    {
        return new(vector);
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
    public static implicit operator ReadOnlyMemory<T>(ReadOnlyVector<T> vector)
    {
        return vector is not null ? vector.Data : default;
    }
}
