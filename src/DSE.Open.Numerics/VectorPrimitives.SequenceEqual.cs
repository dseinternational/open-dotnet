// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public static partial class VectorPrimitives
{
    public static bool SequenceEqual<T>(this scoped in Span<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEquatable<T>?
    {
        if (x.Length != y.Length)
        {
            return false;
        }

        return Tensor.SequenceEqual(
            new TensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y));
    }

    public static bool SequenceEqual<T>(this scoped in ReadOnlySpan<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEquatable<T>?
    {
        if (x.Length != y.Length)
        {
            return false;
        }

        return Tensor.SequenceEqual(
            new ReadOnlyTensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y));
    }

    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return SequenceEqual(x.AsSpan(), y);
    }

    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return SequenceEqual(x.AsSpan(), y.AsSpan());
    }
}
