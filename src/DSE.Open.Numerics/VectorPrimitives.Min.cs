// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T Min<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.Min(x);
    }

    public static T Min<T>([NotNull] this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Min(x.AsSpan());
    }

    public static void Min<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Min(x, y, destination);
    }

    public static void Min<T>([NotNull] this IReadOnlyVector<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Min(x.AsSpan(), y, destination);
    }

    public static void Min<T>(ReadOnlySpan<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Min(x, y, destination);
    }

    public static void Min<T>([NotNull] this IReadOnlyVector<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Min(x.AsSpan(), y, destination);
    }
}
