// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Floor<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Floor(x, destination);
    }

    public static void Floor<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Floor(x.AsSpan(), destination);
    }

    public static void Floor<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Floor(x, destination.AsSpan());
    }

    public static Vector<T> Floor<T>(this IReadOnlyVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Floor(x, destination.AsSpan());
        return destination;
    }

    public static void FloorInPlace<T>(this IVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Floor(x, x.AsSpan());
    }

    public static void Ceiling<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Ceiling(x, destination);
    }

    public static void Ceiling<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Ceiling(x.AsSpan(), destination);
    }

    public static void Ceiling<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Ceiling(x, destination.AsSpan());
    }

    public static Vector<T> Ceiling<T>(this IReadOnlyVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Ceiling(x, destination.AsSpan());
        return destination;
    }

    public static void CeilingInPlace<T>(this IVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Ceiling(x, x.AsSpan());
    }

    public static void Round<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Round(x, destination);
    }

    public static void Round<T>(ReadOnlySpan<T> x, int digits, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Round(x, digits, destination);
    }

    public static void Round<T>(ReadOnlySpan<T> x, MidpointRounding mode, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Round(x, mode, destination);
    }

    public static void Round<T>(ReadOnlySpan<T> x, int digits, MidpointRounding mode, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Round(x, digits, mode, destination);
    }

    public static void Round<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Round(x.AsSpan(), destination);
    }

    public static void Round<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Round(x, destination.AsSpan());
    }

    public static Vector<T> Round<T>(this IReadOnlyVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Round(x, destination.AsSpan());
        return destination;
    }

    public static void RoundInPlace<T>(this IVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Round(x, x.AsSpan());
    }

    public static void Truncate<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Truncate(x, destination);
    }

    public static void Truncate<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Truncate(x.AsSpan(), destination);
    }

    public static void Truncate<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Truncate(x, destination.AsSpan());
    }

    public static Vector<T> Truncate<T>(this IReadOnlyVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Truncate(x, destination.AsSpan());
        return destination;
    }

    public static void TruncateInPlace<T>(this IVector<T> x)
        where T : struct, IFloatingPoint<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Truncate(x, x.AsSpan());
    }
}
