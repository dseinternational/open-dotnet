// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static partial class SeriesExtensions
{
    public static int IndexOf<T>(this IReadOnlySeries<T> series, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.IndexOf(value);
    }

    public static int IndexOf<T>(this IReadOnlySeries<T> series, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.IndexOf(value);
    }

    public static int LastIndexOf<T>(this IReadOnlySeries<T> series, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.LastIndexOf(value);
    }
}
