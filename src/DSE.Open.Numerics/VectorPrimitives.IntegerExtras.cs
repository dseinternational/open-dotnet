// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    // -------- DivRem --------

    /// <summary>Element-wise <c>(x / y, x % y)</c>.</summary>

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

    /// <summary>Element-wise <c>(x / y, x % y)</c>.</summary>

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

    /// <summary>Element-wise <c>(x / y, x % y)</c>.</summary>

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

    /// <summary>Element-wise <c>x % y</c>.</summary>

    public static void Remainder<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    /// <summary>Element-wise <c>x % y</c>.</summary>

    public static void Remainder<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    /// <summary>Element-wise <c>x % y</c>.</summary>

    public static void Remainder<T>(T x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IModulusOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Remainder(x, y, destination);
    }

    /// <summary>Element-wise IEEE 754 remainder.</summary>

    public static void Ieee754Remainder<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    /// <summary>Element-wise IEEE 754 remainder.</summary>

    public static void Ieee754Remainder<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    /// <summary>Element-wise IEEE 754 remainder.</summary>

    public static void Ieee754Remainder<T>(T x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Ieee754Remainder(x, y, destination);
    }

    // -------- Increment / Decrement --------

    /// <summary>Element-wise <c>x + 1</c>.</summary>

    public static void Increment<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IIncrementOperators<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Increment(x, destination);
    }

    /// <summary>Element-wise <c>x - 1</c>.</summary>

    public static void Decrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IDecrementOperators<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Decrement(x, destination);
    }

    // -------- IEEE helpers: BitDecrement, BitIncrement, ScaleB, ILogB --------

    /// <summary>Element-wise <c>BitDecrement</c> (next-down floating-point value).</summary>

    public static void BitDecrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitDecrement(x, destination);
    }

    /// <summary>Element-wise <c>BitIncrement</c> (next-up floating-point value).</summary>

    public static void BitIncrement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitIncrement(x, destination);
    }

    /// <summary>Element-wise <c>x * 2^n</c>.</summary>

    public static void ScaleB<T>(ReadOnlySpan<T> x, int n, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ScaleB(x, n, destination);
    }

    /// <summary>Element-wise integer base-2 logarithm.</summary>

    public static void ILogB<T>(ReadOnlySpan<T> x, in Span<int> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ILogB(x, destination);
    }
}
