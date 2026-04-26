// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise square root.</summary>
    public static void Sqrt<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sqrt(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise square root.</summary>

    public static void Sqrt<T>(this IReadOnlySeries<T> x, ISeries<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sqrt(x, destination.AsSpan());
    }

    /// <summary>Element-wise square root.</summary>

    public static Series<T> Sqrt<T>(this IReadOnlySeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Sqrt(), x);
    }

    /// <summary>Element-wise square root (in place).</summary>

    public static void SqrtInPlace<T>(this ISeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sqrt(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise cube root.</summary>

    public static void Cbrt<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cbrt(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise cube root.</summary>

    public static Series<T> Cbrt<T>(this IReadOnlySeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Cbrt(), x);
    }

    /// <summary>Element-wise cube root (in place).</summary>

    public static void CbrtInPlace<T>(this ISeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cbrt(x.AsSpan(), x.AsSpan());
    }
}
