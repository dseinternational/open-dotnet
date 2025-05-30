// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T Variance<T>(IEnumerable<T> sequence, T? mean = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return Vector.Variance(span, mean);
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
