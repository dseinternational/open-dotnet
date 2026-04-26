// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>n</c>-th root.</summary>
    public static void RootN<T>(this IReadOnlySeries<T> x, int n, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.RootN(x.Vector.AsSpan(), n, destination);
    }

    /// <summary>Element-wise <c>sqrt(x*x + y*y)</c>.</summary>

    public static void Hypot<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Hypot(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>1 / sqrt(x)</c>.</summary>

    public static void ReciprocalSqrt<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ReciprocalSqrt(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise approximate reciprocal of the square root (<c>1/sqrt(x)</c>).</summary>

    public static void ReciprocalSqrtEstimate<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ReciprocalSqrtEstimate(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>1 / x</c>.</summary>

    public static void Reciprocal<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Reciprocal(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise approximate reciprocal (<c>1/x</c>).</summary>

    public static void ReciprocalEstimate<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ReciprocalEstimate(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise approximate <c>(x * y) + addend</c>.</summary>

    public static void MultiplyAddEstimate<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        IReadOnlySeries<T> addend,
        Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(addend);
        VectorPrimitives.MultiplyAddEstimate(
            x.Vector.AsSpan(), y.Vector.AsSpan(), addend.Vector.AsSpan(), destination);
    }
}
