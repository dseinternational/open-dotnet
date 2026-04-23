// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the product of the elements in a sequence.
    /// </summary>
    public static T Product<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.Product(x);
    }

    /// <summary>
    /// Computes the product of the elements in a vector.
    /// </summary>
    public static T Product<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Product(x.AsSpan());
    }

    /// <summary>
    /// Computes the product of the elementwise sums of two sequences.
    /// </summary>
    public static T ProductOfSums<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.ProductOfSums(x, y);
    }

    /// <summary>
    /// Computes the product of the elementwise sums of two vectors.
    /// </summary>
    public static T ProductOfSums<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return ProductOfSums(x.AsSpan(), y.AsSpan());
    }

    /// <summary>
    /// Computes the product of the elementwise differences of two sequences.
    /// </summary>
    public static T ProductOfDifferences<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.ProductOfDifferences(x, y);
    }

    /// <summary>
    /// Computes the product of the elementwise differences of two vectors.
    /// </summary>
    public static T ProductOfDifferences<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return ProductOfDifferences(x.AsSpan(), y.AsSpan());
    }
}
