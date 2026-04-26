// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>x^y</c>.</summary>
    public static void Pow<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Pow(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x^y</c>.</summary>

    public static Series<T> Pow<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapBinary(x.Vector.Pow(y.Vector), x, y);
    }

    /// <summary>Element-wise <c>x^y</c>.</summary>

    public static void Pow<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Pow(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x^y</c>.</summary>

    public static Series<T> Pow<T>(this IReadOnlySeries<T> x, T y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Pow(y), x);
    }

    /// <summary>Element-wise <c>x^y</c> (in place).</summary>

    public static void PowInPlace<T>(this ISeries<T> x, IReadOnlySeries<T> y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Pow(x.AsSpan(), y.Vector.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise <c>x^y</c> (in place).</summary>

    public static void PowInPlace<T>(this ISeries<T> x, T y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Pow(x.AsSpan(), y, x.AsSpan());
    }
}
