// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

/// <summary>
/// An ordered list of numbers stored in a contiguous block of memory.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(Vector), nameof(Vector.Create))]
public readonly struct Vector<T> : IEquatable<Vector<T>>
    where T : struct, INumber<T>
{
    private readonly Memory<T> _data;

    public Vector(T[] data) : this(new Memory<T>(data))
    {
    }

    public Vector(T[] data, int start, int length) : this(new Memory<T>(data, start, length))
    {
    }

    public Vector(Memory<T> data)
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
        return obj is Vector<T> vector && Equals(vector);
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

    public bool Equals(Vector<T> other)
    {
        return _data.Equals(other._data);
    }

    public bool SequenceEqual(Vector<T> other)
    {
        return _data.Span.SequenceEqual(other._data.Span);
    }

    public T[] ToArray()
    {
        return _data.ToArray();
    }

    public static bool operator ==(Vector<T> left, Vector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector<T> left, Vector<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator Vector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator Vector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyVector<T>(Vector<T> vector)
    {
        return new(vector._data);
    }
}
