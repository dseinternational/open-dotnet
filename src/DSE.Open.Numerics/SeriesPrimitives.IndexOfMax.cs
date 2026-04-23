// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Returns the index of the largest element in <paramref name="x"/>, or -1 if empty.
    /// </summary>
    public static int IndexOfMax<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.IndexOfMax();
    }
}
