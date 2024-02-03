// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static T Mode<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mode<T, T>(sequence);
    }

    public static TResult Mode<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);

        // https://en.wikipedia.org/wiki/Mode_(statistics)
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L738

        throw new NotImplementedException();
    }

    public static T Mode<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mode<T, T>(sequence);
    }

    public static TResult Mode<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}