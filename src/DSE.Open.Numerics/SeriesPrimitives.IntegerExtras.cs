// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void DivRem<T>(
        this IReadOnlySeries<T> left,
        IReadOnlySeries<T> right,
        Span<T> quotientDestination,
        Span<T> remainderDestination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        VectorPrimitives.DivRem(
            left.Vector.AsSpan(), right.Vector.AsSpan(), quotientDestination, remainderDestination);
    }

    public static void DivRem<T>(
        this IReadOnlySeries<T> left,
        T right,
        Span<T> quotientDestination,
        Span<T> remainderDestination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(left);
        VectorPrimitives.DivRem(
            left.Vector.AsSpan(), right, quotientDestination, remainderDestination);
    }

    public static void Remainder<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Remainder(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    public static void Remainder<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Remainder(x.Vector.AsSpan(), y, destination);
    }

    public static void Ieee754Remainder<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Ieee754Remainder(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    public static void Ieee754Remainder<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Ieee754Remainder(x.Vector.AsSpan(), y, destination);
    }

    public static void Increment<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IIncrementOperators<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Increment(x.Vector.AsSpan(), destination);
    }

    public static void Decrement<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IDecrementOperators<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Decrement(x.Vector.AsSpan(), destination);
    }

    public static void BitDecrement<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.BitDecrement(x.Vector.AsSpan(), destination);
    }

    public static void BitIncrement<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.BitIncrement(x.Vector.AsSpan(), destination);
    }

    public static void ScaleB<T>(this IReadOnlySeries<T> x, int n, Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ScaleB(x.Vector.AsSpan(), n, destination);
    }

    public static void ILogB<T>(this IReadOnlySeries<T> x, Span<int> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ILogB(x.Vector.AsSpan(), destination);
    }
}
