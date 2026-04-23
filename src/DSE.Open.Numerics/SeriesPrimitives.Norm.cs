// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the Euclidean (L2) norm of a series: <c>sqrt(sum(x_i^2))</c>.
    /// </summary>
    public static T Norm<T>(this IReadOnlySeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.Norm(x.Vector.AsSpan());
    }
}
