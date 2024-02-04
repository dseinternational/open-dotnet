// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// N-dimensional view over <see cref="Memory{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct MemoryND<T> : IEquatable<MemoryND<T>>
{
    private readonly Memory<T> _memory;
    private readonly uint[] _shape;
    private readonly uint[] _strides;

    /// <summary>
    /// Creates a 1-dimensional <see cref="MemoryND{T}"/> of the specified length.
    /// </summary>
    /// <param name="length"></param>
    public MemoryND(int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0, nameof(length));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, Array.MaxLength, nameof(length));

        _shape = [(uint)length];
        _strides = [1u];

        _memory = new T[length];
    }

    public MemoryND(Memory2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = SpanND.GetStrides(_shape);

        if (!elements.TryGetMemory(out _memory))
        {
            throw new InvalidOperationException();
        }
    }

    internal MemoryND(Memory<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty)
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

        _strides = SpanND.GetStrides(shape);

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
        get => _memory.Span[(int)SpanND.GetIndex(_strides, index)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory.Span[(int)SpanND.GetIndex(_strides, index)] = value;
    }

    public Memory<T> Memory => _memory;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_memory.Length;

    public uint Rank => (uint)_shape.Length;

    public override bool Equals(object? obj)
    {
        throw new NotSupportedException();
    }

    public bool Equals(MemoryND<T> other)
    {
        throw new NotSupportedException();
    }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public static bool operator ==(MemoryND<T> left, MemoryND<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MemoryND<T> left, MemoryND<T> right)
    {
        return !(left == right);
    }
}

public static class MemoryND
{
    /// <summary>
    /// Creates a new 2-dimensional view over memory containing copies of the elements
    /// from a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MemoryND<T> Create<T>(T[,] elements)
    {
        return Create(new SpanND<T>(elements));
    }

    /// <summary>
    /// Creates a new 3-dimensional view over memory containing copies of the elements
    /// from a 3-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MemoryND<T> Create<T>(T[,,] elements)
    {
        return Create(new SpanND<T>(elements));
    }

    /// <summary>
    /// Creates a new 4-dimensional view over memory containing copies of the elements
    /// from a 4-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MemoryND<T> Create<T>(T[,,,] elements)
    {
        return Create(new SpanND<T>(elements));
    }

    /// <summary>
    /// Creates a new n-dimensional view over memory containing copies of the elements
    /// from a n-dimensional view over memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MemoryND<T> Create<T>(SpanND<T> elements)
    {
        return new MemoryND<T>(elements.Memory.ToArray(), elements.Shape, false);
    }

    /// <summary>
    /// Creates a new 2-dimensional <see cref="MemoryND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static MemoryND<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
    {
        uint[] d = [d1Length, d2Length];
        return new MemoryND<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="MemoryND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static MemoryND<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new MemoryND<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MemoryND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static MemoryND<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
    {
        return new MemoryND<T>(default, shape, true);
    }
}
