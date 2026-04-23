// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the population standard deviation (square root of the population variance) of a
    /// series of at least one number.
    /// </summary>
    public static T PopulationStandardDeviation<T>(this IReadOnlySeries<T> series)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.PopulationStandardDeviation(series.Vector.AsSpan());
    }

    /// <summary>
    /// Gets the population standard deviation (square root of the population variance) of a
    /// series of at least one number, accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult PopulationStandardDeviation<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, IRootFunctions<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.PopulationStandardDeviation<T, TResult>(series.Vector.AsSpan());
    }
}
