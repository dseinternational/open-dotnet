// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

[CollectionBuilder(typeof(Vector), nameof(Vector.Create))]
public readonly struct Vector<T> : IVector<T, Vector<T>>, IEquatable<Vector<T>>
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

    static Vector<T> IVector<T, Vector<T>>.Create(Span<T> data)
    {
        return new Vector<T>(data.ToArray());
    }

    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span;
    }

    public int Length => _data.Length;

    ReadOnlySpan<T> IReadOnlyVector<T, Vector<T>>.Span => Span;

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

    static Vector<T> IReadOnlyVector<T, Vector<T>>.Create(ReadOnlySpan<T> data)
    {
        // only option is to copy?
        throw new NotImplementedException();
    }

    public static bool operator ==(Vector<T> left, Vector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector<T> left, Vector<T> right)
    {
        return !(left == right);
    }
}
