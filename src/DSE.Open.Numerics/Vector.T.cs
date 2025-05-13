// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable sequence of values of known length of type <typeparamref name="T"/>
/// with value equality semantics. Optionally named, labelled or categorised for use with
/// a <see cref="DataFrame"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(Vector), nameof(Create))]
[JsonConverter(typeof(VectorJsonConverter))]
public sealed class Vector<T> : Vector, IVector<T>, IReadOnlyVector<T>
{
    private readonly Memory<T> _vector;
    private readonly Memory<KeyValuePair<string, T>> _categories;
    private IDictionary<string, T>? _categoriesLookup;

    /// <summary>
    /// Creates an empty series.
    /// </summary>
    public Vector() : this(Memory<T>.Empty, null, null, null)
    {
    }

    public Vector(
        T[] vector,
        string? name = null,
        Memory<Variant> labels = default,
        Memory<KeyValuePair<string, T>> categories = default)
        : this(vector.AsMemory(), name, labels, categories)
    {
    }

    public Vector(
        Memory<T> vector,
        string? name = null,
        Memory<Variant> labels = default,
        Memory<KeyValuePair<string, T>> categories = default)
        : base(VectorDataTypeHelper.GetSeriesDataType<T>(), typeof(T), vector.Length, name, labels)
    {
        _vector = vector;
        _categories = categories;
        // TODO: check if categories are valid
    }

    public Memory<T> Data => _vector;

    ReadOnlyMemory<T> IReadOnlyVector<T>.Data => _vector;

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _vector.Span[index] = value;
    }

    public bool HasCategories => _categories.Length > 0
        || (_categoriesLookup is not null && _categoriesLookup.Count > 0);

    public IDictionary<string, T> Categories
    {
        get
        {
            if (_categoriesLookup is null)
            {
                _categoriesLookup = new Dictionary<string, T>(_categories.Span.Length);

                foreach (var kvp in _categories.Span)
                {
                    _categoriesLookup[kvp.Key] = kvp.Value;
                }
            }

            return _categoriesLookup;
        }
    }

    IReadOnlyDictionary<string, T> IReadOnlyVector<T>.Categories => Categories.AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return _vector.Span;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    ReadOnlySpan<T> IReadOnlyVector<T>.AsReadOnlySpan()
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
        ReadOnlyMemory<KeyValuePair<string, T>> categories =
            _categoriesLookup is not null && _categoriesLookup.Count > 0
                ? _categoriesLookup.ToArray()
                : _categories;

        return new ReadOnlyVector<T>(_vector, Name, Labels, categories);
    }

    ReadOnlyVector<T> IVector<T>.AsReadOnly()
    {
        return AsReadOnly();
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

    public MemoryEnumerator<T> GetEnumerator()
    {
        return _vector.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable((ReadOnlyMemory<T>)Data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Slice(int start)
    {
        return _vector[start..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Slice(int start, int length)
    {
        return _vector.Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ReadOnlyMemory<T> IReadOnlyVector<T>.Slice(int start, int length)
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
    public static implicit operator Memory<T>(Vector<T>? vector)
    {
        return vector is not null ? vector._vector : default;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
