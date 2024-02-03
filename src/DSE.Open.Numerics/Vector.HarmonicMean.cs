// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static T HarmonicMean<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TResult HarmonicMean<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);

        // https://en.wikipedia.org/wiki/Harmonic_mean
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L545

        throw new NotImplementedException();
    }

    public static T HarmonicMean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return HarmonicMean<T, T>(sequence);
    }

    public static TResult HarmonicMean<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: consider accumulating method to avoid TResult overflow

        throw new NotImplementedException();
    }
}
