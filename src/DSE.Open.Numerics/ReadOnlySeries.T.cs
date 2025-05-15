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
    public static readonly ReadOnlySeries<T> Empty = new([], null, null, null);

    private readonly ReadOnlyVector<T> _vector;
    private readonly ReadOnlyDataLabelCollection<T> _labels;

    internal ReadOnlySeries(
        [NotNull] ReadOnlyVector<T> vector,
        string? name,
        Index? index,
        ReadOnlyDataLabelCollection<T>? labels)
        : base(vector, name, index)
    {
        _vector = vector;
        _labels = labels ?? [];
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector[index];
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;

#pragma warning restore CA1033 // Interface methods should be callable by child types

    public ReadOnlyDataLabelCollection<T> Labels => _labels;

    IReadOnlyDataLabelCollection<T> IReadOnlySeries<T>.Labels => Labels;

    public IEnumerable<string> LabelledValues => new SeriesLabelEnumerable<T>(this);

    // protected ReadOnlyVector<T> Vector => _vector;

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
        return new(_vector[start..], Name, null, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start, int length)
    {
        return new(_vector.Slice(start, length), Name, null, null);
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
        return new(vector, null, null, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlySeries<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector, null, null, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlySeries<T> vector)
    {
        return vector is not null ? (ReadOnlyMemory<T>)vector._vector : default;
    }
}
