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
    /// <param name="mean"></param>
    /// <returns></returns>
    public static T Variance<T>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return Variance<T, T>(span, mean);
    }

    public static TResult Variance<T, TResult>(ReadOnlySpan<T> span, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (span.Length < 2)
        {
            NumericsArgumentException.Throw();
        }

        // TODO: incomplete

        // Consider: different algorithms for enumerable/in-memory sequences?

        // https://en.wikipedia.org/wiki/Variance
        // https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance

        // Examples:
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L874
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L208

        var variance = TResult.AdditiveIdentity;

        var t = TResult.CreateChecked(span[0]);

        for (var i = 1; i < span.Length; i++)
        {
            var t1 = TResult.CreateChecked(span[i]);
            t += t1;
            var diff = (TResult.CreateChecked(i + 1) * t1) - t;
            variance += diff * diff / TResult.CreateChecked((i + 1.0) * i);
        }

        return variance / TResult.CreateChecked(span.Length - 1);
    }
}
