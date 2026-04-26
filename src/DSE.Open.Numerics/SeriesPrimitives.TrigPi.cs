// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>sin(pi*x)</c>.</summary>
    public static void SinPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.SinPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>cos(pi*x)</c>.</summary>

    public static void CosPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.CosPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>tan(pi*x)</c>.</summary>

    public static void TanPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.TanPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>(sin(pi*x), cos(pi*x))</c>.</summary>

    public static void SinCosPi<T>(
        this IReadOnlySeries<T> x,
        Span<T> sinDestination,
        Span<T> cosDestination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.SinCosPi(x.Vector.AsSpan(), sinDestination, cosDestination);
    }

    /// <summary>Element-wise <c>asin(x)/pi</c>.</summary>

    public static void AsinPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.AsinPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>acos(x)/pi</c>.</summary>

    public static void AcosPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.AcosPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>atan(x)/pi</c>.</summary>

    public static void AtanPi<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.AtanPi(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>atan2(x, y)/pi</c>.</summary>

    public static void Atan2Pi<T>(
        this IReadOnlySeries<T> y,
        IReadOnlySeries<T> x,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Atan2Pi(y.Vector.AsSpan(), x.Vector.AsSpan(), destination);
    }
}
