// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public partial class Vector
{
    /// <summary>
    /// Gets the sample variance of a sequence of at least two numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <param name="mean">An optional precomputed arithmetic mean. When supplied, the
    /// first pass over <paramref name="span"/> is skipped and the supplied value is
    /// used as the centre of the squared deviations.</param>
    public static T Variance<T>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return Variance<T, T>(span, mean);
    }

    /// <summary>
    /// Gets the sample variance of a sequence of at least two numbers, accumulating
    /// into <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult">The type used for accumulation and the returned result.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <param name="mean">An optional precomputed arithmetic mean. When supplied, the
    /// first pass over <paramref name="span"/> is skipped and the supplied value is
    /// used as the centre of the squared deviations.</param>
    public static TResult Variance<T, TResult>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (span.Length < 2)
        {
            NumericsArgumentException.Throw();
        }

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

        return sumSquaredDeviations / TResult.CreateChecked(span.Length - 1);
    }
}
