// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public partial class VectorPrimitives
{
    public static T Max<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.Max(x);
    }

    public static T Max<T>([NotNull] this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Max(x.AsSpan());
    }

    public static void Max<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Max(x, y, destination);
    }

    public static void Max<T>([NotNull] this IReadOnlyVector<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Max(x.AsSpan(), y, destination);
    }

    public static void Max<T>(ReadOnlySpan<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Max(x, y, destination);
    }

    public static void Max<T>([NotNull] this IReadOnlyVector<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Max(x.AsSpan(), y, destination);
    }
}
