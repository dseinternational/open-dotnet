// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Gets the sample variance of a sequence of at least two numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <param name="mean"></param>
    /// <returns></returns>
    public static T PopulationVariance<T>(ReadOnlySpan<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return PopulationVariance<T, T>(sequence, mean);
    }

    public static TResult PopulationVariance<T, TResult>(ReadOnlySpan<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }

    public static T PopulationVariance<T>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return PopulationVariance(span, mean);
        }

        throw new NotImplementedException();
    }

    public static TResult PopulationVariance<T, TResult>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
