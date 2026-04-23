// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Sinh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sinh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Sinh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Sinh();
    }

    public static void Cosh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Cosh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Cosh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Cosh();
    }

    public static void Tanh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Tanh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Tanh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Tanh();
    }

    public static void Asinh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Asinh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Asinh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Asinh();
    }

    public static void Acosh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Acosh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Acosh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Acosh();
    }

    public static void Atanh<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Atanh(x.Vector.AsSpan(), destination);
    }

    public static Series<T> Atanh<T>(this IReadOnlySeries<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Atanh();
    }
}
