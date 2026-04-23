// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the sample standard deviation (square root of the sample variance) of a
    /// series of at least two numbers.
    /// </summary>
    public static T StandardDeviation<T>(this IReadOnlySeries<T> series)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.StandardDeviation(series.Vector.AsSpan());
    }

    /// <summary>
    /// Gets the sample standard deviation (square root of the sample variance) of a
    /// series of at least two numbers, accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult StandardDeviation<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, IRootFunctions<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.StandardDeviation<T, TResult>(series.Vector.AsSpan());
    }
}
