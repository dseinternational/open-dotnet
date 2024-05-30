// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct SpanVector<T>
    where T : struct, INumber<T>
{
    private readonly Span<T> _data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SpanVector(T[] data) : this(new Span<T>(data))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SpanVector(Span<T> data)
    {
        _data = data;
    }

    internal Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data[index] = value;
    }

    public int Length => _data.Length;

    public Span<T>.Enumerator GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator SpanVector<T>(T[]? array)
    {
        return new(array.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator SpanVector<T>(Span<T> span)
    {
        return new(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlySpanVector<T>(SpanVector<T> span)
    {
        return new(span._data);
    }

    [Obsolete("Equals() on SpanVector will always throw an exception.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        throw new NotSupportedException();
    }

    [Obsolete("GetHashCode() on SpanVector will always throw an exception.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }
}
