// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T GeometricMean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return GeometricMean<T, T>(sequence);
    }

    public static TResult GeometricMean<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: consider accumulating method to avoid TResult overflow

        throw new NotImplementedException();
    }
}
