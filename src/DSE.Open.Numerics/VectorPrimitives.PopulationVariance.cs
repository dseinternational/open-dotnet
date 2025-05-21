// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the sample variance of a sequence of at least two numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <param name="mean"></param>
    /// <returns></returns>
    public static T PopulationVariance<T>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return PopulationVariance<T, T>(span, mean);
    }

    public static TResult PopulationVariance<T, TResult>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
