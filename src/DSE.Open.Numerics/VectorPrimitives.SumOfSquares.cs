// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the sum of the squares of the elements in a sequence.
    /// </summary>
    public static T SumOfSquares<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.SumOfSquares(x);
    }

    /// <summary>
    /// Computes the sum of the squares of the elements in a vector.
    /// </summary>
    public static T SumOfSquares<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return SumOfSquares(x.AsSpan());
    }
}
