// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the Euclidean (L2) norm of a sequence: <c>sqrt(sum(x_i^2))</c>.
    /// </summary>
    public static T Norm<T>(ReadOnlySpan<T> x)
        where T : struct, IRootFunctions<T>
    {
        return TensorPrimitives.Norm(x);
    }

    /// <summary>
    /// Computes the Euclidean (L2) norm of a vector: <c>sqrt(sum(x_i^2))</c>.
    /// </summary>
    public static T Norm<T>(this IReadOnlyVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Norm(x.AsSpan());
    }
}
