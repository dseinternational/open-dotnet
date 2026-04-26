// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>
    public static void CopySign<T>(ReadOnlySpan<T> x, T sign, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.CopySign(x, sign, destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static void CopySign<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> sign, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == sign.Length && sign.Length == destination.Length);
        TensorPrimitives.CopySign(x, sign, destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static void CopySign<T>(this IReadOnlyVector<T> x, T sign, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        CopySign(x.AsSpan(), sign, destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static void CopySign<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> sign, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        CopySign(x.AsSpan(), sign.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static Vector<T> CopySign<T>(this IReadOnlyVector<T> x, T sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        CopySign(x, sign, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise <c>CopySign</c>: returns <c>x</c> with the sign of <c>y</c>.</summary>

    public static Vector<T> CopySign<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        var destination = Vector.Create<T>(x.Length);
        CopySign(x, sign, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static void Sign<T>(ReadOnlySpan<T> x, in Span<int> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sign(x, destination);
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static void Sign<T>(this IReadOnlyVector<T> x, in Span<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sign(x.AsSpan(), destination);
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static void Sign<T>(this IReadOnlyVector<T> x, IVector<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sign(x, destination.AsSpan());
    }

    /// <summary>Element-wise sign (-1, 0, or 1).</summary>

    public static Vector<int> Sign<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<int>(x.Length);
        Sign(x, destination.AsSpan());
        return destination;
    }
}
