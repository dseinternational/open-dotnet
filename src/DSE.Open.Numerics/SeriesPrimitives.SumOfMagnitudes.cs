// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the sum of the absolute values of the elements in a series.
    /// </summary>
    public static T SumOfMagnitudes<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.SumOfMagnitudes();
    }
}
