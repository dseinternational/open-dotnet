// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the geometric mean of a non-empty series, computed as <c>exp(mean(log(x)))</c>
    /// for numerical stability.
    /// </summary>
    public static T GeometricMean<T>(this IReadOnlySeries<T> series)
        where T : struct, ILogarithmicFunctions<T>, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.GeometricMean(series.Vector.AsSpan());
    }

    /// <summary>
    /// Gets the geometric mean of a non-empty series, accumulating into
    /// <typeparamref name="TResult"/>, computed as <c>exp(mean(log(x)))</c> for
    /// numerical stability.
    /// </summary>
    public static TResult GeometricMean<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, ILogarithmicFunctions<TResult>, IExponentialFunctions<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.GeometricMean<T, TResult>(series.Vector.AsSpan());
    }
}
