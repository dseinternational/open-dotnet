// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Divides the data into <paramref name="n"/> continuous intervals with equal
    /// probability and returns the <c>n - 1</c> cut points separating the intervals.
    /// </summary>
    public static ReadOnlySpan<T> Quantiles<T>(this IReadOnlySeries<T> series, int n = 4)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return VectorPrimitives.Quantiles(series.Vector.AsSpan(), n);
    }
}
