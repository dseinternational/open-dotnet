// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Returns the maximum element.</summary>
    public static T Max<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Max();
    }

    /// <summary>Returns the maximum element.</summary>

    public static void Max<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Max(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Returns the maximum element.</summary>

    public static void Max<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Max(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Returns the maximum element.</summary>

    public static void Max<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Max(x.Vector.AsSpan(), y, destination);
    }
}
