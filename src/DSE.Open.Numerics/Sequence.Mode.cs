// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>Reserved — mode is not yet implemented.</summary>
    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static T Mode<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        throw new NotImplementedException();
    }

    /// <summary>Reserved — mode is not yet implemented.</summary>
    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TResult Mode<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        throw new NotImplementedException();
    }
}
