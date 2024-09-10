// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// <b>[Experimental]</b> A multi-dimensional view over <see cref="Memory{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ReadOnlyMultiMemory<T> : IEquatable<ReadOnlyMultiMemory<T>>
{
    private readonly ReadOnlyMemory<T> _elements;
    private readonly uint[] _shape;
    private readonly uint[] _strides;

    /// <summary>
    /// Creates a 1-dimensional <see cref="ReadOnlyMultiMemory{T}"/> of the specified length.
    /// Elements are initialised to <see langword="default" />.
    /// </summary>
    /// <param name="length"></param>
    public ReadOnlyMultiMemory(uint length)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (uint)Array.MaxLength, nameof(length));

        _shape = [length];
        _strides = [1u];

        _elements = new T[length];
    }

    /// <summary>
    /// Creates a 1-dimensional <see cref="ReadOnlyMultiMemory{T}"/> of the specified length.
    /// Elements are initialised to <see langword="default" />.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="initialValue"></param>
    public ReadOnlyMultiMemory(uint length, T initialValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (uint)Array.MaxLength, nameof(length));

        _shape = [length];
        _strides = [1u];

        Memory<T> data = new T[length];
        data.Span.Fill(initialValue);
        _elements = data;
    }

    public ReadOnlyMultiMemory(ReadOnlyMemory2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = MultiMemory.GetStrides(_shape);

        if (!elements.TryGetMemory(out _elements))
        {
            throw new InvalidOperationException();
        }
    }

    public ReadOnlyMultiMemory(ReadOnlyMemory<T> elements)
        : this(elements, [1], false)
    {
    }

    public ReadOnlyMultiMemory(ReadOnlyMemory<T> elements, ReadOnlySpan<uint> shape)
        : this(elements, shape, false)
    {
    }

    public ReadOnlyMultiMemory(ReadOnlySpan<uint> shape)
        : this(default, shape, true)
    {
    }

    internal ReadOnlyMultiMemory(ReadOnlyMemory<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty)
    {
        _shape = shape.ToArray();

        var size = TensorPrimitives.Product<uint>(_shape);

        _strides = MultiMemory.GetStrides(shape);

        if (createIfEmpty && elements.IsEmpty && size > 0)
        {
            _elements = new T[size];
            return;
        }

        if (elements.Length != size)
        {
            throw new ArgumentException("The number of elements in the memory must match the product of the shape");
        }

        _elements = elements;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements.Span[(int)MultiMemory.GetIndex(_strides, index)];
    }

    public ReadOnlyMultiSpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(_elements.Span, _shape, false);
    }

    public ReadOnlyMemory<T> Elements => _elements;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_elements.Length;

    public uint Rank => (uint)_shape.Length;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyMultiMemory<T> other && Equals(other);
    }

    public bool Equals(ReadOnlyMultiMemory<T> other)
    {
        return _elements.Equals(other._elements)
            && _shape.AsSpan().SequenceEqual(other._shape.AsSpan());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in _elements.Span)
        {
            hash.Add(i);
        }

        foreach (var i in _shape)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(ReadOnlyMultiMemory<T> left, ReadOnlyMultiMemory<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyMultiMemory<T> left, ReadOnlyMultiMemory<T> right)
    {
        return !(left == right);
    }
}
