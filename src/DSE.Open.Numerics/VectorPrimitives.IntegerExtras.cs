// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    // -------- DivRem --------

    public static void DivRem<T>(
        ReadOnlySpan<T> left,
        ReadOnlySpan<T> right,
        in Span<T> quotientDestination,
        in Span<T> remainderDestination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(
            left.Length == right.Length
            && right.Length == quotientDestination.Length
            && quotientDestination.Length == remainderDestination.Length);
        TensorPrimitives.DivRem(left, right, quotientDestination, remainderDestination);
    }

    public static void DivRem<T>(
        ReadOnlySpan<T> left,
        T right,
        in Span<T> quotientDestination,
        in Span<T> remainderDestination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(
            left.Length == quotientDestination.Length
            && quotientDestination.Length == remainderDestination.Length);
        TensorPrimitives.DivRem(left, right, quotientDestination, remainderDestination);
    }

    public static void DivRem<T>(
        T left,
        ReadOnlySpan<T> right,
        in Span<T> quotientDestination,
        in Span<T> remainderDestination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(
            right.Length == quotientDestination.Length
            && quotientDestination.Length == remainderDestination.Length);
        TensorPrimitives.DivRem(left, right, quotientDestination, remainderDestination);
    }

    // -------- Remainder --------

    public static void Remainder<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    public static void Remainder<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    public static void Remainder<T>(T x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    public static void Ieee754Remainder<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    public static void Ieee754Remainder<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    public static void Ieee754Remainder<T>(T x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    // -------- Increment / Decrement --------

    public static void Increment<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IIncrementOperators<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Increment(x, destination);
    }

    public static void Decrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IDecrementOperators<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Decrement(x, destination);
    }

    // -------- IEEE helpers: BitDecrement, BitIncrement, ScaleB, ILogB --------

    public static void BitDecrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitDecrement(x, destination);
    }

    public static void BitIncrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitIncrement(x, destination);
    }

    public static void ScaleB<T>(ReadOnlySpan<T> x, int n, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ScaleB(x, n, destination);
    }

    public static void ILogB<T>(ReadOnlySpan<T> x, in Span<int> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ILogB(x, destination);
    }
}
