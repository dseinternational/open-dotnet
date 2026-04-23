// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Clamp<T>(this IReadOnlySeries<T> x, T min, T max, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Clamp(x.Vector.AsSpan(), min, max, destination);
    }

    public static void Clamp<T>(this IReadOnlySeries<T> x, T min, T max, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Clamp(x, min, max, destination.AsSpan());
    }

    public static Series<T> Clamp<T>(this IReadOnlySeries<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Clamp(min, max);
    }

    public static void ClampInPlace<T>(this ISeries<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Clamp(x.AsSpan(), min, max, x.AsSpan());
    }
}
