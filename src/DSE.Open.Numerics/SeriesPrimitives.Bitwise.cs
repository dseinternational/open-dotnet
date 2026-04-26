// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise bitwise AND.</summary>
    public static void BitwiseAnd<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.BitwiseAnd(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise bitwise AND.</summary>

    public static void BitwiseAnd<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.BitwiseAnd(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise bitwise OR.</summary>

    public static void BitwiseOr<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.BitwiseOr(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise bitwise OR.</summary>

    public static void BitwiseOr<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.BitwiseOr(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise bitwise XOR.</summary>

    public static void Xor<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.Xor(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise bitwise XOR.</summary>

    public static void Xor<T>(this IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Xor(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise bitwise NOT (one's complement).</summary>

    public static void OnesComplement<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IBitwiseOperators<T, T, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.OnesComplement(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise count of leading zero bits.</summary>

    public static void LeadingZeroCount<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.LeadingZeroCount(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise count of trailing zero bits.</summary>

    public static void TrailingZeroCount<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.TrailingZeroCount(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise population count (number of set bits).</summary>

    public static void PopCount<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.PopCount(x.Vector.AsSpan(), destination);
    }

    /// <summary>
    /// Computes the total number of set bits across all elements in <paramref name="x"/>.
    /// </summary>
    public static long PopCountTotal<T>(this IReadOnlySeries<T> x)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.PopCountTotal(x.Vector.AsSpan());
    }

    /// <summary>Element-wise left shift.</summary>

    public static void ShiftLeft<T>(this IReadOnlySeries<T> x, int shiftAmount, Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ShiftLeft(x.Vector.AsSpan(), shiftAmount, destination);
    }

    /// <summary>Element-wise arithmetic right shift.</summary>

    public static void ShiftRightArithmetic<T>(this IReadOnlySeries<T> x, int shiftAmount, Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ShiftRightArithmetic(x.Vector.AsSpan(), shiftAmount, destination);
    }

    /// <summary>Element-wise logical (unsigned) right shift.</summary>

    public static void ShiftRightLogical<T>(this IReadOnlySeries<T> x, int shiftAmount, Span<T> destination)
        where T : struct, IShiftOperators<T, int, T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.ShiftRightLogical(x.Vector.AsSpan(), shiftAmount, destination);
    }

    /// <summary>Element-wise rotate-left.</summary>

    public static void RotateLeft<T>(this IReadOnlySeries<T> x, int rotateAmount, Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.RotateLeft(x.Vector.AsSpan(), rotateAmount, destination);
    }

    /// <summary>Element-wise rotate-right.</summary>

    public static void RotateRight<T>(this IReadOnlySeries<T> x, int rotateAmount, Span<T> destination)
        where T : struct, IBinaryInteger<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.RotateRight(x.Vector.AsSpan(), rotateAmount, destination);
    }
}
