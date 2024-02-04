// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// <b>[Experimental]</b> A multi-dimensional view over <see cref="Memory{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct MultiMemory<T> : IEquatable<MultiMemory<T>>
{
    private readonly Memory<T> _memory;
    private readonly uint[] _shape;
    private readonly uint[] _strides;

    /// <summary>
    /// Creates a 1-dimensional <see cref="MultiMemory{T}"/> of the specified length.
    /// Elements are initialised to <see langword="default" />.
    /// </summary>
    /// <param name="length"></param>
    public MultiMemory(uint length)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (uint)Array.MaxLength, nameof(length));

        _shape = [length];
        _strides = [1u];

        _memory = new T[length];
    }

    /// <summary>
    /// Creates a 1-dimensional <see cref="MultiMemory{T}"/> of the specified length.
    /// Elements are initialised to <see langword="default" />.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="initialValue"></param>
    public MultiMemory(uint length, T initialValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (uint)Array.MaxLength, nameof(length));

        _shape = [length];
        _strides = [1u];

        _memory = new T[length];
        _memory.Span.Fill(initialValue);
    }

    public MultiMemory(Memory2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = MultiSpan.GetStrides(_shape);

        if (!elements.TryGetMemory(out _memory))
        {
            throw new InvalidOperationException();
        }
    }

    public MultiMemory(Memory<T> elements)
        : this(elements, [1], false)
    {
    }

    public MultiMemory(Memory<T> elements, ReadOnlySpan<uint> shape)
        : this(elements, shape, false)
    {
    }

    internal MultiMemory(Memory<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty)
    {
        _shape = new uint[shape.Length];

        long size = 1;

        for (var i = 0; i < shape.Length; i++)
        {
            if (shape[i] < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), "Dimensions must be non-negative");
            }

            _shape[i] = shape[i];
            size *= shape[i];
        }

        _strides = MultiSpan.GetStrides(shape);

        if (createIfEmpty && elements.IsEmpty && size > 0)
        {
            _memory = new T[size];
            return;
        }

        if (elements.Length != size)
        {
            throw new ArgumentException("The number of elements in the memory must match the product of the shape");
        }

        _memory = elements;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory.Span[(int)MultiSpan.GetIndex(_strides, index)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory.Span[(int)MultiSpan.GetIndex(_strides, index)] = value;
    }

    public Memory<T> Memory => _memory;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_memory.Length;

    public uint Rank => (uint)_shape.Length;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return obj is MultiMemory<T> other && Equals(other);
    }

    public bool Equals(MultiMemory<T> other)
    {
        return _memory.Equals(other._memory)
            && _shape.AsSpan().SequenceEqual(other._shape.AsSpan());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in _memory.Span)
        {
            hash.Add(i);
        }

        foreach (var i in _shape)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(MultiMemory<T> left, MultiMemory<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MultiMemory<T> left, MultiMemory<T> right)
    {
        return !(left == right);
    }
}

public static class MultiMemory
{
    /// <summary>
    /// Creates a new 1-dimensional view over memory containing copies of the elements
    /// from a 1-dimensional array. To create without copying, use
    /// <see cref="MultiMemory{T}(Memory{T})"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 2-dimensional view over memory containing copies of the elements
    /// from a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 3-dimensional view over memory containing copies of the elements
    /// from a 3-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 4-dimensional view over memory containing copies of the elements
    /// from a 4-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,,,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new n-dimensional view over memory containing copies of the elements
    /// from a n-dimensional view over memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(MultiSpan<T> elements)
    {
        return new MultiMemory<T>(elements.Memory.ToArray(), elements.Shape, false);
    }

    /// <summary>
    /// Creates a new 2-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
    {
        uint[] d = [d1Length, d2Length];
        return new MultiMemory<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new MultiMemory<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
    {
        return new MultiMemory<T>(default, shape, true);
    }
}
