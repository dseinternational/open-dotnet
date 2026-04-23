// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the dot product of two sequences.
    /// </summary>
    public static T Dot<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.Dot(x, y);
    }

    /// <summary>
    /// Computes the dot product of two vectors.
    /// </summary>
    public static T Dot<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return Dot(x.AsSpan(), y.AsSpan());
    }

    /// <summary>
    /// Computes the dot product of a vector and a span.
    /// </summary>
    public static T Dot<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Dot(x.AsSpan(), y);
    }
}
