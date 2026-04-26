// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Returns the population variance of <paramref name="sequence"/>. When
    /// <paramref name="mean"/> is supplied the centring pass is skipped.
    /// Currently implemented only for sequences that expose a span.
    /// </summary>
    public static T PopulationVariance<T>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return VectorPrimitives.PopulationVariance(span, mean);
        }

        throw new NotImplementedException();
    }

    /// <summary>Reserved — accumulating population variance is not yet implemented.</summary>
    public static TResult PopulationVariance<T, TResult>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
