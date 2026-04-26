// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>Extension methods over <see cref="IReadOnlySeries{T}"/>.</summary>
public static partial class SeriesExtensions
{
    /// <summary>Returns the zero-based index of the first occurrence of <paramref name="value"/> in <paramref name="series"/>, or <c>-1</c> when absent.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="series"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this IReadOnlySeries<T> series, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.IndexOf(value);
    }

    /// <summary>Returns the zero-based index of the first occurrence of the sequence <paramref name="value"/> in <paramref name="series"/>, or <c>-1</c> when absent.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="series"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this IReadOnlySeries<T> series, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.IndexOf(value);
    }

    /// <summary>Returns the zero-based index of the last occurrence of <paramref name="value"/> in <paramref name="series"/>, or <c>-1</c> when absent.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="series"/> is <see langword="null"/>.</exception>
    public static int LastIndexOf<T>(this IReadOnlySeries<T> series, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.LastIndexOf(value);
    }
}
