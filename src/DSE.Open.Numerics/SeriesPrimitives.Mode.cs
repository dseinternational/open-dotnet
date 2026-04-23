// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the most frequent value (the mode) of a non-empty series. Ties are broken
    /// by returning the first value to reach the peak frequency as the input is scanned
    /// left to right.
    /// </summary>
    public static T Mode<T>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Mode(series.Vector.AsSpan());
    }

    /// <summary>
    /// Gets the most frequent value (the mode) of a non-empty series, projected to
    /// <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult Mode<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Mode<T, TResult>(series.Vector.AsSpan());
    }
}
