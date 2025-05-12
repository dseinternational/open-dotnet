// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlySeries<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.Vector.Span.IndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlySeries<T> vector, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.Vector.Span.IndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this IReadOnlySeries<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.Vector.Span.LastIndexOf(value);
    }
}
