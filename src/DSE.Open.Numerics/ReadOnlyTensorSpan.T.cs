// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA2225 // Operator overloads have named alternates
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
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
    private readonly ReadOnlyMultiSpan<T> _memory;

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
        _memory = span;
    }

    public T this[ReadOnlySpan<uint> index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _memory[index];
    }

    public ReadOnlyMultiSpan<T> Span => _memory;

    public ReadOnlySpan<uint> Shape => _memory.Shape;

    public ReadOnlySpan<uint> Strides => _memory.Strides;

    public uint Length => _memory.Length;

    public uint Rank => _memory.Length;


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
        return left._memory == right._memory;
    }

    public static bool operator !=(ReadOnlyTensorSpan<T> left, ReadOnlyTensorSpan<T> right)
    {
        return !(left == right);
    }
}
