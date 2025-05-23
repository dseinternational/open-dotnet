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
///. Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(ReadOnlySeries), nameof(Create))]
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public class ReadOnlySeries<T> : ReadOnlySeries, IReadOnlySeries<T>
    where T : IEquatable<T>
{
    public static readonly ReadOnlySeries<T> Empty = new([]);

    private readonly ReadOnlyVector<T> _vector;
    private CategorySet<T>? _categories;

    public ReadOnlySeries([NotNull] ReadOnlyVector<T> vector, string? name = null, CategorySet<T>? categories = null)
        : base(vector, name)
    {
        _vector = vector;
        _categories = categories;

        if (_categories is not null)
        {
            NumericsArgumentException.ThrowIfNotInSet(vector, _categories);
        }
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector[index];
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public new ReadOnlyVector<T> Vector => _vector;

    [MemberNotNullWhen(true, nameof(_categories))]
    public override bool IsCategorical => _categories is not null && !_categories.IsEmpty;

    public CategorySet<T> Categories => _categories ??= [];

    public override VectorValue GetVectorValue(int index)
    {
        return VectorValue.FromValue(this[index]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return _vector.AsSpan();
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

    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return _vector.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable((ReadOnlyMemory<T>)_vector).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start)
    {
        return new(_vector[start..], Name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start, int length)
    {
        return new(_vector.Slice(start, length), Name);
    }

    IReadOnlySeries<T> IReadOnlySeries<T>.Slice(int start, int length)
    {
        return Slice(start, length);
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
    public static implicit operator ReadOnlySeries<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
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
        return vector is not null ? (ReadOnlyMemory<T>)vector._vector : default;
    }
}
