// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the dot product of two series.
    /// </summary>
    public static T Dot<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.Dot(x.Vector.AsSpan(), y.Vector.AsSpan());
    }
}
