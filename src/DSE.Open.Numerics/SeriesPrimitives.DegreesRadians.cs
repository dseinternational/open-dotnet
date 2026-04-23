// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void DegreesToRadians<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.DegreesToRadians(x.Vector.AsSpan(), destination);
    }

    public static Series<T> DegreesToRadians<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.DegreesToRadians();
    }

    public static void RadiansToDegrees<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.RadiansToDegrees(x.Vector.AsSpan(), destination);
    }

    public static Series<T> RadiansToDegrees<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.RadiansToDegrees();
    }
}
