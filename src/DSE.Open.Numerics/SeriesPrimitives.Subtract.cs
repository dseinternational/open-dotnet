// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitivesSubtract
{
    public static void Subtract<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        VectorPrimitives.Subtract(x.Vector, y, destination);
    }

    public static void Subtract<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Subtract(x, y.Vector.AsSpan(), destination);
        Subtract(x, y.Vector.AsSpan(), destination);
    }

    public static void Subtract<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Subtract(x, y, destination.AsSpan());
    }

    public static void Subtract<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == destination.Length);
        VectorPrimitives.Subtract(x.Vector, y, destination);
    }

    public static void Subtract<T>(this IReadOnlySeries<T> x, T y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Subtract(x, y, destination.AsSpan());
    }

    public static void SubtractInPlace<T>(this ISeries<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Subtract(x, y, x.AsSpan());
    }

    public static void SubtractInPlace<T>(this ISeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        SubtractInPlace(x, y.Vector.AsSpan());
    }

    public static void SubtractInPlace<T>(this ISeries<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Subtract(x, y, x.AsSpan());
    }
}
