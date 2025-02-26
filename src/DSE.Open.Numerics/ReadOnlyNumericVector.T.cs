// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

[CollectionBuilder(typeof(ReadOnlyNumericVector), nameof(ReadOnlyNumericVector.Create))]
public readonly struct ReadOnlyNumericVector<T> : IEquatable<ReadOnlyNumericVector<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMemory<T> _data;

    public ReadOnlyNumericVector(T[] data)
        : this(new ReadOnlyMemory<T>(data))
    {
    }

    public ReadOnlyNumericVector(T[] data, int start, int length)
        : this(new ReadOnlyMemory<T>(data, start, length))
    {
    }

    public ReadOnlyNumericVector(ReadOnlyMemory<T> data)
    {
        _data = data;
    }

    public ReadOnlySpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span;
    }

    public int Length => _data.Length;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index];
    }

    public override bool Equals(object? obj)
    {
        return obj is NumericVector<T> vector && Equals(vector);
    }

    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }

    public bool Equals(ReadOnlyNumericVector<T> other)
    {
        return _data.Equals(other._data);
    }

    public static bool operator ==(ReadOnlyNumericVector<T> left, ReadOnlyNumericVector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyNumericVector<T> left, ReadOnlyNumericVector<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyNumericVector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyNumericVector<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
    }
}
