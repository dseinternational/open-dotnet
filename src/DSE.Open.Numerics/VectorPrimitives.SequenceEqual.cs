// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;


public static partial class VectorPrimitives
{
    /// <summary>Returns <see langword="true"/> when the two sequences contain the same elements in order.</summary>
    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.AsSpan().SequenceEqual(y);
    }

    /// <summary>Returns <see langword="true"/> when the two sequences contain the same elements in order.</summary>

    public static bool SequenceEqual<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return x.AsSpan().SequenceEqual(y.AsSpan());
    }
}
