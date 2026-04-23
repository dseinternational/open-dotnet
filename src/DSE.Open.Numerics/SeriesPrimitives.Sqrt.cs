// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Sqrt<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sqrt(x.Vector.AsSpan(), destination);
    }

    public static void Sqrt<T>(this IReadOnlySeries<T> x, ISeries<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sqrt(x, destination.AsSpan());
    }

    public static Series<T> Sqrt<T>(this IReadOnlySeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Sqrt(), x);
    }

    public static void SqrtInPlace<T>(this ISeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sqrt(x.AsSpan(), x.AsSpan());
    }

    public static void Cbrt<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cbrt(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Cbrt<T>(this IReadOnlySeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Cbrt(), x);
    }

    public static void CbrtInPlace<T>(this ISeries<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cbrt(x.AsSpan(), x.AsSpan());
    }
}
