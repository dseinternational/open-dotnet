// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    public static int IndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    public static int IndexOf<T>(this IReadOnlyVector<T> vector, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    public static int LastIndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().LastIndexOf(value);
    }
}
