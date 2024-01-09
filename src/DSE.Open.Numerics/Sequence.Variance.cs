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
    /// <typeparam name="T"></typeparam>
    /// <param name="samples"></param>
    /// <param name="mean"></param>
    /// <returns></returns>
    public static T Variance<T>(ReadOnlySpan<T> samples, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (samples.Length < 2)
        {
            NumericsException.Throw();
        }

        // TODO: incomplete

        // Consider: different algorithms for enumerable/in-memory sequences?

        // https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance

        // Examples:
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L874
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L208

        var variance = T.AdditiveIdentity;

        var t = samples[0];

        for (var i = 1; i < samples.Length; i++)
        {
            t += samples[i];
            var diff = (T.CreateChecked(i + 1) * samples[i]) - t;
            variance += diff * diff / T.CreateChecked((i + 1.0) * i);
        }

        return variance / T.CreateChecked(samples.Length - 1);
    }

    public static T Variance<T>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan<T>(out var span))
        {
            return Variance(span, mean);
        }

        throw new NotImplementedException();
    }
}
