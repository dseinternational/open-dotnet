// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void MultiplyAdd<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        IReadOnlySeries<T> addend,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(addend);
        VectorPrimitives.MultiplyAdd(x.Vector.AsSpan(), y.Vector.AsSpan(), addend.Vector.AsSpan(), destination);
    }

    public static void MultiplyAdd<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        T addend,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.MultiplyAdd(x.Vector.AsSpan(), y.Vector.AsSpan(), addend, destination);
    }

    public static void FusedMultiplyAdd<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        IReadOnlySeries<T> addend,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(addend);
        VectorPrimitives.FusedMultiplyAdd(
            x.Vector.AsSpan(), y.Vector.AsSpan(), addend.Vector.AsSpan(), destination);
    }

    public static void Lerp<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        T amount,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Lerp(x.Vector.AsSpan(), y.Vector.AsSpan(), amount, destination);
    }

    public static void Lerp<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        IReadOnlySeries<T> amount,
        Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(amount);
        VectorPrimitives.Lerp(x.Vector.AsSpan(), y.Vector.AsSpan(), amount.Vector.AsSpan(), destination);
    }

    public static void AddMultiply<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        IReadOnlySeries<T> multiplier,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(multiplier);
        VectorPrimitives.AddMultiply(x.Vector.AsSpan(), y.Vector.AsSpan(), multiplier.Vector.AsSpan(), destination);
    }

    public static void AddMultiply<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        T multiplier,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.AddMultiply(x.Vector.AsSpan(), y.Vector.AsSpan(), multiplier, destination);
    }
}
