// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// N-dimensional view over a <see cref="Span{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct SpanND<T>
{
    private readonly Span<T> _memory;
    private readonly uint[] _shape;
    private readonly uint[] _strides;

    public SpanND(T[] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape = [(uint)elements.Length];

        _strides = SpanND.GetStrides(_shape);

        _memory = elements;
    }

    public SpanND(Span<T> elements)
    {
        _shape = [(uint)elements.Length];

        _strides = SpanND.GetStrides(_shape);

        _memory = elements;
    }

    public SpanND(T[,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape = [(uint)elements.GetLength(0), (uint)elements.GetLength(1)];

        _strides = SpanND.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public SpanND(T[,,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape =
        [
            (uint)elements.GetLength(0),
            (uint)elements.GetLength(1),
            (uint)elements.GetLength(2)
        ];

        _strides = SpanND.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public SpanND(T[,,,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape =
        [
            (uint)elements.GetLength(0),
            (uint)elements.GetLength(1),
            (uint)elements.GetLength(2),
            (uint)elements.GetLength(3)
        ];

        _strides = SpanND.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public SpanND(T[,,,,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape =
        [
            (uint)elements.GetLength(0),
            (uint)elements.GetLength(1),
            (uint)elements.GetLength(2),
            (uint)elements.GetLength(3),
            (uint)elements.GetLength(4)
        ];

        _strides = SpanND.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public SpanND(Span2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = SpanND.GetStrides(_shape);

        if (!elements.TryGetSpan(out _memory))
        {
            _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), (int)elements.Length);
        }
    }

    internal SpanND(Span<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty)
    {
        _shape = shape.ToArray();

        var size = TensorPrimitives.Product<uint>(_shape);

        _strides = SpanND.GetStrides(shape);

        if (createIfEmpty && elements.IsEmpty && size > 0)
        {
            _memory = new T[size];
            return;
        }

        if (elements.Length != size)
        {
            throw new ArgumentException("The number of elements in the span must match the product of the shape");
        }

        _memory = elements;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory[(int)SpanND.GetIndex(_strides, index)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory[(int)SpanND.GetIndex(_strides, index)] = value;
    }

    public Span<T> Memory => _memory;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_memory.Length;

    public uint Rank => (uint)_shape.Length;

    public override bool Equals(object? obj)
    {
        throw new NotSupportedException();
    }

    public bool Equals(SpanND<T> other)
    {
        throw new NotSupportedException();
    }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public static bool operator ==(SpanND<T> left, SpanND<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SpanND<T> left, SpanND<T> right)
    {
        return !(left == right);
    }
}

public static class SpanND
{
    /// <summary>
    /// Creates a new 2-dimensional <see cref="SpanND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static SpanND<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
    {
        uint[] d = [d1Length, d2Length];
        return new SpanND<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="SpanND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static SpanND<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new SpanND<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="SpanND{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static SpanND<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
    {
        return new SpanND<T>(default, shape, true);
    }

    /// <summary>
    /// Gets the set of strides that can be used to calculate the offset of n-shape in a 1-dimensional layout
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="reverseStride"></param>
    /// <returns></returns>
    public static uint[] GetStrides(ReadOnlySpan<uint> shape, bool reverseStride = false)
    {
        var strides = new uint[shape.Length];

        if (shape.Length == 0)
        {
            return strides;
        }

        var stride = 1u;

        if (reverseStride)
        {
            for (var i = 0; i < strides.Length; i++)
            {
                strides[i] = stride;
                stride *= shape[i];
            }
        }
        else
        {
            for (var i = strides.Length - 1; i >= 0; i--)
            {
                strides[i] = stride;
                stride *= shape[i];
            }
        }

        return strides;
    }

    /// <summary>
    /// Calculates the 1-d index for n-d indices in layout specified by strides.
    /// </summary>
    /// <param name="strides"></param>
    /// <param name="indices"></param>
    /// <param name="startFromDimension"></param>
    /// <returns></returns>
    public static uint GetIndex(ReadOnlySpan<uint> strides, ReadOnlySpan<uint> indices, uint startFromDimension = 0)
    {
        Debug.Assert(strides.Length == indices.Length);

        var index = 0u;

        for (var i = startFromDimension; i < indices.Length; i++)
        {
            index += strides[(int)i] * indices[(int)i];
        }

        return index;
    }
}
