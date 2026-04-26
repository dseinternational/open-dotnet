// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise sine.</summary>
    public static void Sin<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sin(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise sine.</summary>

    public static Series<T> Sin<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Sin(), x);
    }

    /// <summary>Element-wise cosine.</summary>

    public static void Cos<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cos(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise cosine.</summary>

    public static Series<T> Cos<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Cos(), x);
    }

    /// <summary>Element-wise tangent.</summary>

    public static void Tan<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Tan(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise tangent.</summary>

    public static Series<T> Tan<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Tan(), x);
    }

    /// <summary>Element-wise sine and cosine.</summary>

    public static void SinCos<T>(
        this IReadOnlySeries<T> x,
        Span<T> sinDestination,
        Span<T> cosDestination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.SinCos(x.Vector.AsSpan(), sinDestination, cosDestination);
    }

    /// <summary>Element-wise inverse sine.</summary>

    public static void Asin<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Asin(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise inverse sine.</summary>

    public static Series<T> Asin<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Asin(), x);
    }

    /// <summary>Element-wise inverse cosine.</summary>

    public static void Acos<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Acos(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise inverse cosine.</summary>

    public static Series<T> Acos<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Acos(), x);
    }

    /// <summary>Element-wise inverse tangent.</summary>

    public static void Atan<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Atan(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise inverse tangent.</summary>

    public static Series<T> Atan<T>(this IReadOnlySeries<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Atan(), x);
    }

    /// <summary>Element-wise <c>atan2(x, y)</c>.</summary>

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
