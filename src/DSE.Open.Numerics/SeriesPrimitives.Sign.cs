// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>
    public static void CopySign<T>(this IReadOnlySeries<T> x, T sign, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.CopySign(x.Vector.AsSpan(), sign, destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static void CopySign<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> sign, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        VectorPrimitives.CopySign(x.Vector.AsSpan(), sign.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static Series<T> CopySign<T>(this IReadOnlySeries<T> x, T sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.CopySign(sign), x);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static Series<T> CopySign<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        return WrapBinary(x.Vector.CopySign(sign.Vector), x, sign);
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static void Sign<T>(this IReadOnlySeries<T> x, Span<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sign(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static void Sign<T>(this IReadOnlySeries<T> x, ISeries<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sign(x, destination.AsSpan());
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static Series<int> Sign<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapTypeChange<T, int>(x.Vector.Sign(), x);
    }
}
