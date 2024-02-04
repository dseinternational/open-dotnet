// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
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

}
