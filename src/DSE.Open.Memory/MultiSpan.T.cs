// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Memory;

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// <b>[Experimental]</b> A multi-dimensional view over a <see cref="Span{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct MultiSpan<T>
{
    private readonly Span<T> _elements;
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = elements;
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = elements;
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
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

        _strides = MultiMemory.GetStrides(_shape);

        _elements = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), elements.Length);
    }

    public MultiSpan(Span2D<T> elements)
    {
        _shape = [(uint)elements.Height, (uint)elements.Width];

        _strides = MultiMemory.GetStrides(_shape);

        if (!elements.TryGetSpan(out _elements))
        {
            _elements = MemoryMarshal.CreateSpan(ref elements.DangerousGetReference(), (int)elements.Length);
        }
    }

    public MultiSpan(ReadOnlySpan<uint> shape)
        : this(default, shape, true)
    {
    }

    internal MultiSpan(Span<T> elements, ReadOnlySpan<uint> shape, bool createIfEmpty, bool createUninitialized = false)
    {
        _shape = shape.ToArray();

        var size = TensorPrimitives.Product<uint>(_shape);

        _strides = MultiMemory.GetStrides(shape);

        if (createIfEmpty && elements.IsEmpty && size > 0)
        {
            _elements = createUninitialized
                ? GC.AllocateUninitializedArray<T>((int)size, pinned: false)
                : new T[size];
            return;
        }

        if (elements.Length != size)
        {
            throw new ArgumentException("The number of elements in the span must match the product of the shape");
        }

        _elements = elements;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements[(int)MultiMemory.GetIndex(_strides, index)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _elements[(int)MultiMemory.GetIndex(_strides, index)] = value;
    }

    public Span<T> Elements => _elements;

    public ReadOnlySpan<uint> Shape => _shape;

    public ReadOnlySpan<uint> Strides => _strides;

    public uint Length => (uint)_elements.Length;

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
        return left._elements == right._elements
            && left._shape.AsSpan().SequenceEqual(right._shape.AsSpan());
    }

    public static bool operator !=(MultiSpan<T> left, MultiSpan<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MultiSpan<T>(T[] array)
    {
        return new MultiSpan<T>(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyMultiSpan<T>(MultiSpan<T> elements)
    {
        return new ReadOnlyMultiSpan<T>(elements._elements, elements.Shape, false);
    }
}
