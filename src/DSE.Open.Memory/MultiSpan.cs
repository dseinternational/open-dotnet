// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
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
/// <b>[Experimental]</b> A multi-dimensional view over a <see cref="Span{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct MultiSpan<T>
{
    private readonly Span<T> _memory;
    private readonly uint[] _shape;
    private readonly uint[] _strides;

    /// <summary>
    /// Creates a 1-dimensional view over an array. Changes will be reflected
    /// in the original array.
    /// </summary>
    /// <param name="elements">The elements over which to create a 1-dimensional
    /// <see cref="MultiSpan{T}"/>.</param>
    public MultiSpan(T[] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape = [(uint)elements.Length];

        _strides = MultiSpan.GetStrides(_shape);

        _memory = elements;
    }

    /// <summary>
    /// Creates a 1-dimensional view over a span. Changes will be reflected
    /// in the original span.
    /// </summary>
    /// <param name="elements">The elements over which to create a 1-dimensional
    /// <see cref="MultiSpan{T}"/>.</param>
    public MultiSpan(Span<T> elements)
    {
        _shape = [(uint)elements.Length];

        _strides = MultiSpan.GetStrides(_shape);

        _memory = elements;
    }

    /// <summary>
    /// Creates a 2-dimensional view over an 2-dimensional array. Changes will
    /// be reflected in the original array.
    /// </summary>
    /// <param name="elements">The elements over which to create a 2-dimensional
    /// <see cref="MultiSpan{T}"/>.</param>
    public MultiSpan(T[,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape = [(uint)elements.GetLength(0), (uint)elements.GetLength(1)];

        _strides = MultiSpan.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    /// <summary>
    /// Creates a 3-dimensional view over an 3-dimensional array. Changes will
    /// be reflected in the original array.
    /// </summary>
    /// <param name="elements">The elements over which to create a 3-dimensional
    /// <see cref="MultiSpan{T}"/>.</param>
    public MultiSpan(T[,,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape =
        [
            (uint)elements.GetLength(0),
            (uint)elements.GetLength(1),
            (uint)elements.GetLength(2)
        ];

        _strides = MultiSpan.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    /// <summary>
    /// Creates a 4-dimensional view over an 4-dimensional array. Changes will
    /// be reflected in the original array.
    /// </summary>
    /// <param name="elements">The elements over which to create a 4-dimensional
    /// <see cref="MultiSpan{T}"/>.</param>
    public MultiSpan(T[,,,] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        _shape =
        [
            (uint)elements.GetLength(0),
            (uint)elements.GetLength(1),
            (uint)elements.GetLength(2),
            (uint)elements.GetLength(3)
        ];

        _strides = MultiSpan.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public MultiSpan(T[,,,,] elements)
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

        _strides = MultiSpan.GetStrides(_shape);

        _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public MultiSpan(Span2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = MultiSpan.GetStrides(_shape);

        if (!elements.TryGetSpan(out _memory))
        {
            _memory = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), (int)elements.Length);
        }
    }

    internal MultiSpan(Span<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty)
    {
        _shape = shape.ToArray();

        var size = TensorPrimitives.Product<uint>(_shape);

        _strides = MultiSpan.GetStrides(shape);

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
        get => _memory[(int)MultiSpan.GetIndex(_strides, index)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _memory[(int)MultiSpan.GetIndex(_strides, index)] = value;
    }

    public Span<T> Memory => _memory;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_memory.Length;

    public uint Rank => (uint)_shape.Length;


#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Equals() on MultiSpan<T> will always throw an exception. Use == instead.")]
    public override bool Equals(object? obj)
    {
        throw new NotSupportedException($"{nameof(MultiSpan<T>)}.Equals(object) is not supported.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("GetHashCode() on MultiSpan<T> will always throw an exception.")]
    public override int GetHashCode()
    {
        throw new NotSupportedException($"{nameof(MultiSpan<T>)}.GetHashCode() is not supported.");
    }

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

    public static bool operator ==(MultiSpan<T> left, MultiSpan<T> right)
    {
        return left._memory == right._memory
            && left._shape.AsSpan().SequenceEqual(right._shape.AsSpan());
    }

    public static bool operator !=(MultiSpan<T> left, MultiSpan<T> right)
    {
        return !(left == right);
    }
}

public static class MultiSpan
{
    /// <summary>
    /// Creates a new 2-dimensional <see cref="MultiSpan{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static MultiSpan<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
    {
        uint[] d = [d1Length, d2Length];
        return new MultiSpan<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="MultiSpan{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static MultiSpan<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new MultiSpan<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MultiSpan{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static MultiSpan<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
    {
        return new MultiSpan<T>(default, shape, true);
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
