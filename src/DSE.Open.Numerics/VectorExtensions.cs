// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlyVector<T> vector, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().LastIndexOf(value);
    }
}
