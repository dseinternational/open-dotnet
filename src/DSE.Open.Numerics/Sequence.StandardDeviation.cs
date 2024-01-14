// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T StandardDeviation<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return StandardDeviation<T, T>(sequence);
    }

    public static TResult StandardDeviation<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }

    public static T StandardDeviation<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return StandardDeviation<T, T>(sequence);
    }

    public static TResult StandardDeviation<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
