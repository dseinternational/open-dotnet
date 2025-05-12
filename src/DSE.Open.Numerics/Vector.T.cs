// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
    public static readonly Vector<T> Empty = new([], null, null);

    internal readonly T[] _data;

    internal Vector(
        T[] data,
        string? name = null,
        IReadOnlyDictionary<string, Variant>? annotations = null)
        : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), data.Length, name, annotations)
    {
        _data = data;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data[index] = value;
    }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return _data;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    ReadOnlySpan<T> IReadOnlyVector<T>.AsReadOnlySpan()
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return AsSpan();
    }

    public bool IsReadOnly { get; private set; }

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
        return new ReadOnlyVector<T>(Data);
    }

    protected override ReadOnlyVector CreateReadOnly()
    {
        return AsReadOnly();
    }

    public bool Equals(Vector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    public bool Equals(IVector<T>? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    public bool Equals(IReadOnlyVector<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsSpan().SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return [.. _data];
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IReadOnlyCollection<T>)_data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start)
    {
        return AsSpan()[start..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start, int length)
    {
        return AsSpan().Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ReadOnlySpan<T> IReadOnlyVector<T>.Slice(int start, int length)
    {
        return Slice(start, length);
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
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Memory<T>(Vector<T> vector)
    {
        return vector is not null ? vector._data : default;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
