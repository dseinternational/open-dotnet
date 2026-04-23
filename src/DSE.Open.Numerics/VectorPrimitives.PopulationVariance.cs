// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the population variance (divided by <c>n</c>) of a sequence of at least one number.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <param name="mean">An optional precomputed arithmetic mean. When supplied, the
    /// first pass over <paramref name="span"/> is skipped and the supplied value is
    /// used as the centre of the squared deviations.</param>
    public static T PopulationVariance<T>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return PopulationVariance<T, T>(span, mean);
    }

    /// <summary>
    /// Gets the population variance (divided by <c>n</c>) of a sequence of at least one number,
    /// accumulating into <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult PopulationVariance<T, TResult>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);

        TResult meanResult;

        if (mean.HasValue)
        {
            meanResult = TResult.CreateChecked(mean.Value);
        }
        else
        {
            var sum = TResult.AdditiveIdentity;
            for (var i = 0; i < span.Length; i++)
            {
                sum += TResult.CreateChecked(span[i]);
            }
            meanResult = sum / TResult.CreateChecked(span.Length);
        }

        var sumSquaredDeviations = TResult.AdditiveIdentity;
        for (var i = 0; i < span.Length; i++)
        {
            var diff = TResult.CreateChecked(span[i]) - meanResult;
            sumSquaredDeviations += diff * diff;
        }

        return sumSquaredDeviations / TResult.CreateChecked(span.Length);
    }
}
