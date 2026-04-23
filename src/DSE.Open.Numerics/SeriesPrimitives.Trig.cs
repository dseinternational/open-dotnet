// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Sin<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sin(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Sin<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Sin();
    }

    public static void Cos<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cos(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Cos<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Cos();
    }

    public static void Tan<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Tan(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Tan<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Tan();
    }

    public static void SinCos<T>(
        this IReadOnlySeries<T> x,
        Span<T> sinDestination,
        Span<T> cosDestination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.SinCos(x.Vector.AsSpan(), sinDestination, cosDestination);
    }

    public static void Asin<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Asin(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Asin<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Asin();
    }

    public static void Acos<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Acos(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Acos<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Acos();
    }

    public static void Atan<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Atan(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Atan<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Atan();
    }

    public static void Atan2<T>(
        this IReadOnlySeries<T> y,
        IReadOnlySeries<T> x,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Atan2(y.Vector.AsSpan(), x.Vector.AsSpan(), destination);
    }
}
