// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Gets the sample variance of a series of at least two numbers.
    /// </summary>
    public static T Variance<T>(this IReadOnlySeries<T> series, T? mean = default)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return Vector.Variance(series.Vector.AsSpan(), mean);
    }

    /// <summary>
    /// Gets the sample variance of a series of at least two numbers, accumulating into
    /// <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult Variance<T, TResult>(this IReadOnlySeries<T> series, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return Vector.Variance<T, TResult>(series.Vector.AsSpan(), mean);
    }
}
