// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void BitwiseAnd<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.BitwiseAnd(x, y, destination);
    }

    public static void BitwiseAnd<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitwiseAnd(x, y, destination);
    }

    public static void BitwiseOr<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.BitwiseOr(x, y, destination);
    }

    public static void BitwiseOr<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.BitwiseOr(x, y, destination);
    }

    public static void Xor<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Xor(x, y, destination);
    }

    public static void Xor<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Xor(x, y, destination);
    }

    public static void OnesComplement<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.OnesComplement(x, destination);
    }

    public static void LeadingZeroCount<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.LeadingZeroCount(x, destination);
    }

    public static void TrailingZeroCount<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.TrailingZeroCount(x, destination);
    }

    public static void PopCount<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.PopCount(x, destination);
    }

    /// <summary>
    /// Computes the total number of set bits across all elements in <paramref name="x"/>.
    /// </summary>
    public static long PopCountTotal<T>(ReadOnlySpan<T> x)
        where T : struct, IBinaryInteger<T>
    {
        return TensorPrimitives.PopCount(x);
    }

    /// <summary>
    /// Computes the total number of set bits across all elements in <paramref name="x"/>.
    /// </summary>
    public static long PopCountTotal<T>(this IReadOnlyVector<T> x)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return PopCountTotal(x.AsSpan());
    }

    public static void ShiftLeft<T>(ReadOnlySpan<T> x, int shiftAmount, in Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ShiftLeft(x, shiftAmount, destination);
    }

    public static void ShiftRightArithmetic<T>(ReadOnlySpan<T> x, int shiftAmount, in Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ShiftRightArithmetic(x, shiftAmount, destination);
    }

    public static void ShiftRightLogical<T>(ReadOnlySpan<T> x, int shiftAmount, in Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ShiftRightLogical(x, shiftAmount, destination);
    }

    public static void RotateLeft<T>(ReadOnlySpan<T> x, int rotateAmount, in Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.RotateLeft(x, rotateAmount, destination);
    }

    public static void RotateRight<T>(ReadOnlySpan<T> x, int rotateAmount, in Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.RotateRight(x, rotateAmount, destination);
    }
}
