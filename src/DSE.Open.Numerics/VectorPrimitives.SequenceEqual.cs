// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public static partial class VectorPrimitives
{
    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.AsSpan().SequenceEqual(y);
    }

    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return x.AsSpan().SequenceEqual(y.AsSpan());
    }
}
