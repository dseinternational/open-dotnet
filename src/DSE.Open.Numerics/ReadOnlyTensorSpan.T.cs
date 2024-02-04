// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

/// <summary>
/// <b>[Experimental]</b> A multi-dimensional view over a <see cref="Span{T}"/>
/// of numerical values.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct ReadOnlyTensorSpan<T>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMultiSpan<T> _elements;

    public ReadOnlyTensorSpan(T[] elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(ReadOnlySpan<T> elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(T[,] elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(T[,,] elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(T[,,,] elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(T[,,,,] elements) : this(new ReadOnlyMultiSpan<T>(elements))
    {
    }

    public ReadOnlyTensorSpan(ReadOnlyMultiSpan<T> span)
    {
        _elements = span;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements[index];
    }

    public ReadOnlyMultiSpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements;
    }

    public ReadOnlySpan<T> Elements
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _elements.Elements;
    }

    public ReadOnlySpan<uint> Shape => _elements.Shape;

    public ReadOnlySpan<uint> Strides => _elements.Strides;

    public uint Length => _elements.Length;

    public uint Rank => _elements.Rank;

    public ReadOnlyTensorSpan<T> Add(ReadOnlyTensorSpan<T> other)
    {
        NumericsException.ThrowIfNotSameShape(this, other);

        var result = new TensorSpan<T>(Shape);

        TensorSpan.Add(this, other, result);

        return result;
    }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Equals() on ReadOnlyTensorSpan<T> will always throw an exception. Use == instead.")]
    public override bool Equals(object? obj)
    {
        throw new NotSupportedException($"{nameof(ReadOnlyTensorSpan<T>)}.Equals(object) is not supported.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("GetHashCode() on ReadOnlyTensorSpan<T> will always throw an exception.")]
    public override int GetHashCode()
    {
        throw new NotSupportedException($"{nameof(ReadOnlyTensorSpan<T>)}.GetHashCode() is not supported.");
    }

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

    public static bool operator ==(ReadOnlyTensorSpan<T> left, ReadOnlyTensorSpan<T> right)
    {
        return left._elements == right._elements;
    }

    public static bool operator !=(ReadOnlyTensorSpan<T> left, ReadOnlyTensorSpan<T> right)
    {
        return !(left == right);
    }

    public static ReadOnlyTensorSpan<T> operator +(ReadOnlyTensorSpan<T> left, ReadOnlyTensorSpan<T> right)
    {
        return left.Add(right);
    }
}
