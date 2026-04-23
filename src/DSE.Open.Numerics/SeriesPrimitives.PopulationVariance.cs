// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the population variance (divided by <c>n</c>) of a series of at least one number.
    /// </summary>
    public static T PopulationVariance<T>(this IReadOnlySeries<T> series, T? mean = default)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.PopulationVariance(series.Vector.AsSpan(), mean);
    }

    /// <summary>
    /// Gets the population variance (divided by <c>n</c>) of a series of at least one number,
    /// accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult PopulationVariance<T, TResult>(this IReadOnlySeries<T> series, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.PopulationVariance<T, TResult>(series.Vector.AsSpan(), mean);
    }
}
