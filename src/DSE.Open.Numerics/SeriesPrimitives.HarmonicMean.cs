// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the harmonic mean of a non-empty series: <c>n / Σ(1/xᵢ)</c>.
    /// </summary>
    public static T HarmonicMean<T>(this IReadOnlySeries<T> series)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.HarmonicMean(series.Vector.AsSpan());
    }

    /// <summary>
    /// Gets the harmonic mean of a non-empty series, accumulating into
    /// <typeparamref name="TResult"/>: <c>n / Σ(1/xᵢ)</c>.
    /// </summary>
    public static TResult HarmonicMean<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, IFloatingPoint<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.HarmonicMean<T, TResult>(series.Vector.AsSpan());
    }
}
