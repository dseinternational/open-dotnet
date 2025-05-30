// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T PopulationStandardDeviation<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return PopulationStandardDeviation<T, T>(span);
    }

    public static TResult PopulationStandardDeviation<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
