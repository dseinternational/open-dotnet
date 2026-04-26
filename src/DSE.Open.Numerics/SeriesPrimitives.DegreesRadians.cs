// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise conversion from degrees to radians.</summary>
    public static void DegreesToRadians<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.DegreesToRadians(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise conversion from degrees to radians.</summary>

    public static Series<T> DegreesToRadians<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.DegreesToRadians(), x);
    }

    /// <summary>Element-wise conversion from radians to degrees.</summary>

    public static void RadiansToDegrees<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.RadiansToDegrees(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise conversion from radians to degrees.</summary>

    public static Series<T> RadiansToDegrees<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.RadiansToDegrees(), x);
    }
}
