// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(
        IReadOnlyNumericVector<T> x,
        IReadOnlyNumericVector<T> y,
        INumericVector<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        Subtract(x.Span, y.Span, destination.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(
        IReadOnlyNumericVector<T> x,
        IReadOnlyNumericVector<T> y,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        Subtract(x.Span, y.Span, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Subtract(x, y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(
        IReadOnlyNumericVector<T> x,
        T y,
        INumericVector<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        Subtract(x.Span, y, destination.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(
        IReadOnlyNumericVector<T> x,
        T y,
        Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        Subtract(x.Span, y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(ReadOnlySpan<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        TensorPrimitives.Subtract(x, y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(INumericVector<T> x, IReadOnlyNumericVector<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(y, x);
        SubtractInPlace(x.Span, y.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(Span<T> x, IReadOnlyNumericVector<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(y, x);
        SubtractInPlace(x, y.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Subtract(x, y, x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(INumericVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        SubtractInPlace(x.Span, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(Span<T> x, T y)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Subtract(x, y, x);
    }
}
