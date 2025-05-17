// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public partial class Vector
{
    public static T GeometricMean<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TResult GeometricMean<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);

        // https://en.wikipedia.org/wiki/Geometric_mean
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L526

        throw new NotImplementedException();
    }
}
