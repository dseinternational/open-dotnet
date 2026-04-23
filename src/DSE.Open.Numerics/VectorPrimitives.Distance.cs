// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the Euclidean (L2) distance between two sequences: <c>sqrt(sum((x_i - y_i)^2))</c>.
    /// </summary>
    public static T Distance<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.Distance(x, y);
    }

    /// <summary>
    /// Computes the Euclidean (L2) distance between two vectors.
    /// </summary>
    public static T Distance<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return Distance(x.AsSpan(), y.AsSpan());
    }

    /// <summary>
    /// Computes the number of element-wise inequalities between two sequences.
    /// </summary>
    public static int HammingDistance<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, IEquatable<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.HammingDistance(x, y);
    }

    /// <summary>
    /// Computes the number of element-wise inequalities between two vectors.
    /// </summary>
    public static int HammingDistance<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return HammingDistance(x.AsSpan(), y.AsSpan());
    }

    /// <summary>
    /// Computes the number of differing bits between two integer sequences (popcount of XOR).
    /// </summary>
    public static long HammingBitDistance<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.HammingBitDistance(x, y);
    }

    /// <summary>
    /// Computes the number of differing bits between two integer vectors (popcount of XOR).
    /// </summary>
    public static long HammingBitDistance<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return HammingBitDistance(x.AsSpan(), y.AsSpan());
    }
}
