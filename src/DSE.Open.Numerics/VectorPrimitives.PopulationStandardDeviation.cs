// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the population standard deviation (square root of the population variance) of a
    /// sequence of at least one number.
    /// </summary>
    public static T PopulationStandardDeviation<T>(ReadOnlySpan<T> span)
        where T : struct, IRootFunctions<T>
    {
        return PopulationStandardDeviation<T, T>(span);
    }

    /// <summary>
    /// Gets the population standard deviation (square root of the population variance) of a
    /// sequence of at least one number, accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult PopulationStandardDeviation<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, IRootFunctions<TResult>
    {
        return TResult.Sqrt(PopulationVariance<T, TResult>(span));
    }
}
