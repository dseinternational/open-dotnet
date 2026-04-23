// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the sigmoid activation function element-wise: <c>1 / (1 + exp(-x))</c>.
    /// </summary>
    public static void Sigmoid<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sigmoid(x, destination);
    }

    /// <summary>
    /// Computes the sigmoid activation function element-wise: <c>1 / (1 + exp(-x))</c>.
    /// </summary>
    public static Vector<T> Sigmoid<T>(this IReadOnlyVector<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Sigmoid(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>
    /// Computes the softmax of a sequence: <c>exp(xᵢ) / Σ exp(xⱼ)</c>.
    /// </summary>
    public static void SoftMax<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.SoftMax(x, destination);
    }

    /// <summary>
    /// Computes the softmax of a sequence: <c>exp(xᵢ) / Σ exp(xⱼ)</c>.
    /// </summary>
    public static Vector<T> SoftMax<T>(this IReadOnlyVector<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        SoftMax(x.AsSpan(), destination.AsSpan());
        return destination;
    }
}
