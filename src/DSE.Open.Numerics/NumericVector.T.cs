// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

/// <summary>
/// An ordered list of numbers stored in a contiguous block of memory.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Implements value equality.
/// </remarks>
[CollectionBuilder(typeof(NumericVector), nameof(NumericVector.Create))]
public readonly struct NumericVector<T> : IEquatable<NumericVector<T>>
    where T : struct, INumber<T>
{
    private readonly Memory<T> _data;

    public NumericVector(T[] data) : this(new Memory<T>(data))
    {
    }

    public NumericVector(T[] data, int start, int length) : this(new Memory<T>(data, start, length))
    {
    }

    public NumericVector(Memory<T> data)
    {
        _data = data;
    }

    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span;
    }

    public int Length => _data.Length;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data.Span[index] = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is NumericVector<T> vector && Equals(vector);
    }

    public MemoryEnumerator<T> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }

    public bool Equals(NumericVector<T> other)
    {
        return Equals(other._data.Span);
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return _data.Span.SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return _data.ToArray();
    }

    public static bool operator ==(NumericVector<T> left, NumericVector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NumericVector<T> left, NumericVector<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyNumericVector<T>(NumericVector<T> vector)
    {
        return new(vector._data);
    }
}
