// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Calculates the sample arithmetic mean of a series. If the series is empty,
    /// an <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    public static T Mean<T>(this IReadOnlySeries<T> series)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.Mean();
    }

    /// <summary>
    /// Calculates the sample arithmetic mean of a series, accumulating into
    /// <typeparamref name="TResult"/>. If the series is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    public static TResult Mean<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Mean<T, TResult>(series.Vector.AsSpan());
    }
}
