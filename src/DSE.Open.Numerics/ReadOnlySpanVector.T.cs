// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct ReadOnlySpanVector<T>
    where T : struct, INumber<T>
{
    private readonly ReadOnlySpan<T> _data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpanVector(T[] data) : this(new ReadOnlySpan<T>(data))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpanVector(ReadOnlySpan<T> data)
    {
        _data = data;
    }

    internal ReadOnlySpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data[index];
    }

    public int Length => _data.Length;

    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlySpanVector<T>(T[]? array)
    {
        return new(array.AsSpan());
    }
}
