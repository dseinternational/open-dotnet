// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>
    public static void Clamp<T>(this IReadOnlySeries<T> x, T min, T max, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Clamp(x.Vector.AsSpan(), min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(this IReadOnlySeries<T> x, T min, T max, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Clamp(x, min, max, destination.AsSpan());
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static Series<T> Clamp<T>(this IReadOnlySeries<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Clamp(min, max), x);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c> (in place).</summary>

    public static void ClampInPlace<T>(this ISeries<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Clamp(x.AsSpan(), min, max, x.AsSpan());
    }
}
