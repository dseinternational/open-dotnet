// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static T GeometricMean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        throw new NotImplementedException();
    }

    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TResult GeometricMean<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: consider accumulating method to avoid TResult overflow

        throw new NotImplementedException();
    }
}
