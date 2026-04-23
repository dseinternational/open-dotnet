// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Exp2<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp2(x.Vector.AsSpan(), destination);
    }

    public static void Exp10<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp10(x.Vector.AsSpan(), destination);
    }

    public static void ExpM1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ExpM1(x.Vector.AsSpan(), destination);
    }

    public static void Exp2M1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp2M1(x.Vector.AsSpan(), destination);
    }

    public static void Exp10M1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp10M1(x.Vector.AsSpan(), destination);
    }

    public static void LogP1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.LogP1(x.Vector.AsSpan(), destination);
    }

    public static void Log2P1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log2P1(x.Vector.AsSpan(), destination);
    }

    public static void Log10P1<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log10P1(x.Vector.AsSpan(), destination);
    }
}
