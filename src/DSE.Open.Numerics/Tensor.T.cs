// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// <b>[Experimental]</b> A tensor implemented over a <see cref="MultiMemory{T}"/>
/// view of numerical values.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Tensor<T> : IEquatable<Tensor<T>>
    where T : struct, INumber<T>
{
    private readonly MultiMemory<T> _elements;

    /// <summary>
    /// Creates a 1-dimensional <see cref="Tensor{T}"/> of the specified length.
    /// Elements are initialised to <see langword="default" />.
    /// </summary>
    /// <param name="length"></param>
    public Tensor(uint length) : this (new MultiMemory<T>(length))
    {
    }

    /// <summary>
    /// Creates a 1-dimensional <see cref="Tensor{T}"/> of the specified length.
    /// Elements are initialised to <paramref name="initialValue"/>.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="initialValue"></param>
    public Tensor(uint length, T initialValue) : this (new MultiMemory<T>(length, initialValue))
    {
    }

    public Tensor(Memory2D<T> elements) : this (new MultiMemory<T>(elements))
    {
    }

    public Tensor(Memory<T> elements) : this(new MultiMemory<T>(elements))
    {
    }

    public Tensor(Memory<T> elements, ReadOnlySpan<uint> shape) : this(new MultiMemory<T>(elements, shape))
    {
    }

    public Tensor(ReadOnlySpan<uint> shape) : this(new MultiMemory<T>(shape))
    {
    }

    public Tensor(MultiMemory<T> elements)
    {
        _elements = elements;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _elements[index] = value;
    }

    public TensorSpan<T> TensorSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(_elements.Span);
    }

    public Memory<T> Elements
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
        NumericsException.ThrowIfNotSameShape(this, other);

        var result = Tensor.CreateUninitialized<T>(Shape); // Add will write all locations

        Tensor.Add(this, other, result);

        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj is Tensor<T> tensor && Equals(tensor);
    }

    public bool Equals(Tensor<T> other)
    {
        return _elements.Equals(other._elements);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return _elements.GetHashCode();
    }

    public static bool operator ==(Tensor<T> left, Tensor<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Tensor<T> left, Tensor<T> right)
    {
        return !(left == right);
    }

    public static implicit operator ReadOnlyTensor<T>(Tensor<T> tensor)
    {
        return new(tensor._elements);
    }
}
