// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>x / y</c>.</summary>
    public static void Divide<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        VectorPrimitives.Divide(x.Vector, y, destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Divide(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static Series<T> Divide<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapBinary(x.Vector.Divide(y.Vector), x, y);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        VectorPrimitives.Divide(x.Vector, y, destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlySeries<T> x, T y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static Series<T> Divide<T>(this IReadOnlySeries<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Divide(y), x);
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this ISeries<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this ISeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        DivideInPlace(x, y.Vector.AsSpan());
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this ISeries<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }
}
