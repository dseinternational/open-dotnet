// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void MultiplyAdd<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> addend, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == y.Length && y.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.MultiplyAdd(x, y, addend, destination);
    }

    public static void MultiplyAdd<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, T addend, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MultiplyAdd(x, y, addend, destination);
    }

    public static void MultiplyAdd<T>(ReadOnlySpan<T> x, T y, ReadOnlySpan<T> addend, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.MultiplyAdd(x, y, addend, destination);
    }

    public static void FusedMultiplyAdd<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> addend, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == y.Length && y.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.FusedMultiplyAdd(x, y, addend, destination);
    }

    public static void FusedMultiplyAdd<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, T addend, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.FusedMultiplyAdd(x, y, addend, destination);
    }

    public static void FusedMultiplyAdd<T>(ReadOnlySpan<T> x, T y, ReadOnlySpan<T> addend, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.FusedMultiplyAdd(x, y, addend, destination);
    }

    public static void Lerp<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> amount, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == y.Length && y.Length == amount.Length && amount.Length == destination.Length);
        TensorPrimitives.Lerp(x, y, amount, destination);
    }

    public static void Lerp<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, T amount, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Lerp(x, y, amount, destination);
    }

    public static void Lerp<T>(ReadOnlySpan<T> x, T y, ReadOnlySpan<T> amount, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == amount.Length && amount.Length == destination.Length);
        TensorPrimitives.Lerp(x, y, amount, destination);
    }

    public static void AddMultiply<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> multiplier, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == y.Length && y.Length == multiplier.Length && multiplier.Length == destination.Length);
        TensorPrimitives.AddMultiply(x, y, multiplier, destination);
    }

    public static void AddMultiply<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, T multiplier, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.AddMultiply(x, y, multiplier, destination);
    }

    public static void AddMultiply<T>(ReadOnlySpan<T> x, T y, ReadOnlySpan<T> multiplier, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == multiplier.Length && multiplier.Length == destination.Length);
        TensorPrimitives.AddMultiply(x, y, multiplier, destination);
    }
}
