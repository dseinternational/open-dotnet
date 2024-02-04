// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA2225 // Operator overloads have named alternates
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// <b>[Experimental]</b> A multi-dimensional view over a <see cref="Span{T}"/>
/// of numerical values.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct TensorSpan<T>
    where T : struct, INumber<T>
{
    private readonly MultiSpan<T> _elements;

    public TensorSpan(T[] elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(Span<T> elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(T[,] elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(T[,,] elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(T[,,,] elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(T[,,,,] elements) : this(new MultiSpan<T>(elements))
    {
    }

    public TensorSpan(ReadOnlySpan<uint> shape) : this(new MultiSpan<T>(shape))
    {
    }

    public TensorSpan(MultiSpan<T> span)
    {
        _elements = span;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _elements[index] = value;
    }

    public MultiSpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements;
    }

    public Span<T> Elements
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements.Elements;
    }

    public ReadOnlySpan<uint> Shape => _elements.Shape;

    public ReadOnlySpan<uint> Strides => _elements.Strides;

    public uint Length => _elements.Length;

    public uint Rank => _elements.Rank;

    public TensorSpan<T> Add(ReadOnlyTensorSpan<T> other)
    {
        NumericsException.ThrowIfNotSameShape(this, other);

        var result = new TensorSpan<T>(Shape);

        TensorSpan.Add(this, other, result);

        return result;
    }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Equals() on TensorSpan<T> will always throw an exception. Use == instead.")]
    public override bool Equals(object? obj)
    {
        throw new NotSupportedException($"{nameof(TensorSpan<T>)}.Equals(object) is not supported.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("GetHashCode() on TensorSpan<T> will always throw an exception.")]
    public override int GetHashCode()
    {
        throw new NotSupportedException($"{nameof(TensorSpan<T>)}.GetHashCode() is not supported.");
    }

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

    public static bool operator ==(TensorSpan<T> left, TensorSpan<T> right)
    {
        return left._elements == right._elements;
    }

    public static bool operator !=(TensorSpan<T> left, TensorSpan<T> right)
    {
        return !(left == right);
    }

    public static TensorSpan<T> operator +(TensorSpan<T> left, TensorSpan<T> right)
    {
        return left.Add(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TensorSpan<T>(T[] array)
    {
        return new TensorSpan<T>(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyTensorSpan<T>(TensorSpan<T> elements)
    {
        return new ReadOnlyTensorSpan<T>(elements._elements);
    }
}
