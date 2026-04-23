// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    // -------- Floor --------

    public static void Floor<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Floor(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Floor<T>(this IReadOnlySeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Floor(), x);
    }

    public static void FloorInPlace<T>(this ISeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Floor(x.AsSpan(), x.AsSpan());
    }

    // -------- Ceiling --------

    public static void Ceiling<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Ceiling(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Ceiling<T>(this IReadOnlySeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Ceiling(), x);
    }

    public static void CeilingInPlace<T>(this ISeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Ceiling(x.AsSpan(), x.AsSpan());
    }

    // -------- Round --------

    public static void Round<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Round(x.Vector.AsSpan(), destination);
    }

    public static void Round<T>(this IReadOnlySeries<T> x, int digits, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Round(x.Vector.AsSpan(), digits, destination);
    }

    public static void Round<T>(this IReadOnlySeries<T> x, MidpointRounding mode, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Round(x.Vector.AsSpan(), mode, destination);
    }

    public static void Round<T>(
        this IReadOnlySeries<T> x,
        int digits,
        MidpointRounding mode,
        Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Round(x.Vector.AsSpan(), digits, mode, destination);
    }

    public static Series<T> Round<T>(this IReadOnlySeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Round(), x);
    }

    public static void RoundInPlace<T>(this ISeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Round(x.AsSpan(), x.AsSpan());
    }

    // -------- Truncate --------

    public static void Truncate<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Truncate(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Truncate<T>(this IReadOnlySeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Truncate(), x);
    }

    public static void TruncateInPlace<T>(this ISeries<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Truncate(x.AsSpan(), x.AsSpan());
    }
}
