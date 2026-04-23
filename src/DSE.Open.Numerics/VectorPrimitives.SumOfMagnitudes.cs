// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the sum of the absolute values of the elements in a sequence.
    /// </summary>
    public static T SumOfMagnitudes<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.SumOfMagnitudes(x);
    }

    /// <summary>
    /// Computes the sum of the absolute values of the elements in a vector.
    /// </summary>
    public static T SumOfMagnitudes<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return SumOfMagnitudes(x.AsSpan());
    }
}
