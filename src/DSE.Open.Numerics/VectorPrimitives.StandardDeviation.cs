// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the sample standard deviation (square root of the sample variance) of a
    /// sequence of at least two numbers.
    /// </summary>
    public static T StandardDeviation<T>(ReadOnlySpan<T> span)
        where T : struct, IRootFunctions<T>
    {
        return StandardDeviation<T, T>(span);
    }

    /// <summary>
    /// Gets the sample standard deviation (square root of the sample variance) of a
    /// sequence of at least two numbers, accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult StandardDeviation<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, IRootFunctions<TResult>
    {
        return TResult.Sqrt(Vector.Variance<T, TResult>(span));
    }
}
