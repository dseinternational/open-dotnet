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
[CollectionBuilder(typeof(ReadOnlySeries), nameof(Create))]
[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class ReadOnlySeries<T> : ReadOnlySeries, IReadOnlySeries<T>
{
    public static readonly ReadOnlySeries<T> Empty = new(default);

    private readonly ReadOnlyMemory<T> _vector;
    private readonly Memory<KeyValuePair<string, T>> _categories;
    private IReadOnlyDictionary<string, T>? _categoriesLookup;

    public ReadOnlySeries(
        ReadOnlyMemory<T> vector,
        string? name = null,
        Memory<Variant> labels = default,
        Memory<KeyValuePair<string, T>> categories = default)
        : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), vector.Length, name, labels)
    {
        _vector = vector;
        _categories = categories;
    }

    public ReadOnlyMemory<T> Vector => _vector;

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector.Span[index];
    }

    public IReadOnlyDictionary<string, T> Categories
    {
        get
        {
            if (_categoriesLookup is null)
            {
                var categoriesLookup = new Dictionary<string, T>(_categories.Span.Length);

                foreach (var kvp in _categories.Span)
                {
                    categoriesLookup[kvp.Key] = kvp.Value;
                }

                _categoriesLookup = categoriesLookup;
            }

            return _categoriesLookup;

        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return _vector.Span;
    }

    public override bool Equals(object? obj)
    {
        return obj is Series<T> vector && Equals(vector);
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

    public bool Equals(ReadOnlySeries<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(IReadOnlySeries<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsReadOnlySpan().SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return [.. _vector];
    }

    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return _vector.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        // TODO
        throw new NotImplementedException();
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

    public static bool operator ==(ReadOnlySeries<T>? left, ReadOnlySeries<T>? right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    public static bool operator !=(ReadOnlySeries<T>? left, ReadOnlySeries<T>? right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlySeries<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlySeries<T> vector)
    {
        return vector is not null ? vector._vector : default;
    }
}
