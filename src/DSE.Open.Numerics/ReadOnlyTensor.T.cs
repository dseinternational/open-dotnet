// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// <b>[Experimental]</b> A read-only tensor implemented over a <see cref="ReadOnlyMultiMemory{T}"/>
/// view of numerical values.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ReadOnlyTensor<T> : IEquatable<ReadOnlyTensor<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMultiMemory<T> _elements;

    /// <summary>
    /// Creates a 1-dimensional <see cref="ReadOnlyTensor{T}"/> of the specified length.
    /// Elements are initialised to <paramref name="initialValue"/>.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="initialValue"></param>
    public ReadOnlyTensor(uint length, T initialValue)
        : this(new ReadOnlyMultiMemory<T>(length, initialValue))
    {
    }

    public ReadOnlyTensor(ReadOnlyMemory2D<T> elements)
        : this(new ReadOnlyMultiMemory<T>(elements))
    {
    }

    public ReadOnlyTensor(ReadOnlyMemory<T> elements)
        : this(new ReadOnlyMultiMemory<T>(elements))
    {
    }

    public ReadOnlyTensor(ReadOnlyMemory<T> elements, ReadOnlySpan<uint> shape)
        : this(new ReadOnlyMultiMemory<T>(elements, shape))
    {
    }

    public ReadOnlyTensor(ReadOnlySpan<uint> shape) : this(new ReadOnlyMultiMemory<T>(shape))
    {
    }

    public ReadOnlyTensor(ReadOnlyMultiMemory<T> memory)
    {
        _elements = memory;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements[index];
    }

    public ReadOnlyMultiSpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements.Span;
    }

    public ReadOnlyMemory<T> Elements
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements.Elements;
    }

    public ReadOnlySpan<uint> Shape => _elements.Shape;

    public ReadOnlySpan<uint> Strides => _elements.Strides;

    public uint Length => _elements.Length;

    /// <summary>
    /// The number of indices required to uniquely select each element of the tensor.
    /// </summary>
    public uint Rank => _elements.Rank;

    public Tensor<T> Add(ReadOnlyTensor<T> other)
    {
        var result = Tensor.CreateUninitialized<T>(Shape); // Add will write all locations
        Tensor.Add(this, other, result);
        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyTensor<T> tensor && Equals(tensor);
    }

    public bool Equals(ReadOnlyTensor<T> other)
    {
        return _elements.Equals(other._elements);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return _elements.GetHashCode();
    }

    public static bool operator ==(ReadOnlyTensor<T> left, ReadOnlyTensor<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyTensor<T> left, ReadOnlyTensor<T> right)
    {
        return !(left == right);
    }
}
