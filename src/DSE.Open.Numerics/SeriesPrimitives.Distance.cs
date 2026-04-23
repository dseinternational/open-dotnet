// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the Euclidean (L2) distance between two series.
    /// </summary>
    public static T Distance<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.Distance(x.Vector.AsSpan(), y.Vector.AsSpan());
    }

    /// <summary>
    /// Computes the number of element-wise inequalities between two series.
    /// </summary>
    public static int HammingDistance<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.HammingDistance(x.Vector.AsSpan(), y.Vector.AsSpan());
    }

    /// <summary>
    /// Computes the number of differing bits between two integer series (popcount of XOR).
    /// </summary>
    public static long HammingBitDistance<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.HammingBitDistance(x.Vector.AsSpan(), y.Vector.AsSpan());
    }
}
