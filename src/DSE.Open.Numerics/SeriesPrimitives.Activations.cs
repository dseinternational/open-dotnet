// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the sigmoid activation function element-wise: <c>1 / (1 + exp(-x))</c>.
    /// </summary>
    public static void Sigmoid<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sigmoid(x.Vector.AsSpan(), destination);
    }

    /// <summary>
    /// Computes the sigmoid activation function element-wise: <c>1 / (1 + exp(-x))</c>.
    /// </summary>
    public static Series<T> Sigmoid<T>(this IReadOnlySeries<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Sigmoid(), x);
    }

    /// <summary>
    /// Computes the softmax of a series: <c>exp(xᵢ) / Σ exp(xⱼ)</c>.
    /// </summary>
    public static void SoftMax<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.SoftMax(x.Vector.AsSpan(), destination);
    }

    /// <summary>
    /// Computes the softmax of a series: <c>exp(xᵢ) / Σ exp(xⱼ)</c>.
    /// </summary>
    public static Series<T> SoftMax<T>(this IReadOnlySeries<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.SoftMax(), x);
    }
}
