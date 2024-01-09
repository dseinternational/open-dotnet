// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Divide data into n continuous intervals with equal probability. Returns a list of n - 1
    /// cut points separating the intervals.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="samples"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ReadOnlySpan<T> Quantiles<T>(ReadOnlySpan<T> samples, int n = 4)
        where T : struct, INumberBase<T>
    {
        throw new NotImplementedException();
    }
}
