// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

[CollectionBuilder(typeof(ReadOnlyVector), nameof(ReadOnlyVector.Create))]
public readonly struct ReadOnlyVector<T> : IEquatable<ReadOnlyVector<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMemory<T> _data;

    public ReadOnlyVector(T[] data)
        : this(new ReadOnlyMemory<T>(data))
    {
    }

    public ReadOnlyVector(T[] data, int start, int length)
        : this(new ReadOnlyMemory<T>(data, start, length))
    {
    }

    public ReadOnlyVector(ReadOnlyMemory<T> data)
    {
        _data = data;
    }

    public ReadOnlySpanVector<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(_data.Span);
    }

    public ReadOnlySpan<T> Memory
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
        return obj is Vector<T> vector && Equals(vector);
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

    public bool Equals(ReadOnlyVector<T> other)
    {
        return _data.Equals(other._data);
    }

    public static bool operator ==(ReadOnlyVector<T> left, ReadOnlyVector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyVector<T> left, ReadOnlyVector<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyVector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyVector<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
    }
}
