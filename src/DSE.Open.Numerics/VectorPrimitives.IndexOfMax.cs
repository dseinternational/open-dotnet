// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Returns the index of the largest element in <paramref name="x"/>, or -1 if empty.
    /// </summary>
    public static int IndexOfMax<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.IndexOfMax(x);
    }

    /// <summary>
    /// Returns the index of the largest element in <paramref name="x"/>, or -1 if empty.
    /// </summary>
    public static int IndexOfMax<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IndexOfMax(x.AsSpan());
    }
}
