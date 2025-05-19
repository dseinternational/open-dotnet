// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public partial class Vector
{
    public static T Mode<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return Mode<T, T>(span);
    }

    public static TResult Mode<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);

        // https://en.wikipedia.org/wiki/Mode_(statistics)
        // https://github.com/python/cpython/blob/3.12/Lib/statistics.py#L738

        throw new NotImplementedException();
    }
}
