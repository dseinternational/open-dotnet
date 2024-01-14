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
    public static T Variance<T>(ReadOnlySpan<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        return Variance<T, T>(sequence, mean);
    }

    public static TResult Variance<T, TResult>(ReadOnlySpan<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (sequence.Length < 2)
        {
            NumericsException.Throw();
        }

        // TODO: incomplete

        // Consider: different algorithms for enumerable/in-memory sequences?

        // https://en.wikipedia.org/wiki/Variance
        // https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance

        // Examples:
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L874
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L208

        var variance = TResult.AdditiveIdentity;

        var t = TResult.CreateChecked(sequence[0]);

        for (var i = 1; i < sequence.Length; i++)
        {
            var t1 = TResult.CreateChecked(sequence[i]);
            t += t1;
            var diff = (TResult.CreateChecked(i + 1) * t1) - t;
            variance += diff * diff / TResult.CreateChecked((i + 1.0) * i);
        }

        return variance / TResult.CreateChecked(sequence.Length - 1);
    }

    public static T Variance<T>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return Variance(span, mean);
        }

        throw new NotImplementedException();
    }

    public static TResult Variance<T, TResult>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
