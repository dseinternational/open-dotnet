// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the median (middle value) of a non-empty series.
    /// </summary>
    public static T Median<T>(
        this IReadOnlySeries<T> series,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Median(series.Vector.AsSpan(), median);
    }

    /// <summary>
    /// Gets the median (middle value) of a non-empty series, accumulating into
    /// <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult Median<T, TResult>(
        this IReadOnlySeries<T> series,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumber<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Median<T, TResult>(series.Vector.AsSpan(), median);
    }
}
