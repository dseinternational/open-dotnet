// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the product of the elements in a series.
    /// </summary>
    public static T Product<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Product();
    }

    /// <summary>
    /// Computes the product of the elementwise sums of two series.
    /// </summary>
    public static T ProductOfSums<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.ProductOfSums(x.Vector.AsSpan(), y.Vector.AsSpan());
    }

    /// <summary>
    /// Computes the product of the elementwise differences of two series.
    /// </summary>
    public static T ProductOfDifferences<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return VectorPrimitives.ProductOfDifferences(x.Vector.AsSpan(), y.Vector.AsSpan());
    }
}
