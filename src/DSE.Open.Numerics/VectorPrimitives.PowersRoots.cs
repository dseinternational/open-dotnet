// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void RootN<T>(ReadOnlySpan<T> x, int n, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.RootN(x, n, destination);
    }

    public static void Hypot<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Hypot(x, y, destination);
    }

    public static void ReciprocalSqrt<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ReciprocalSqrt(x, destination);
    }

    public static void ReciprocalSqrtEstimate<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ReciprocalSqrtEstimate(x, destination);
    }

    public static void Reciprocal<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPoint<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Reciprocal(x, destination);
    }

    public static void ReciprocalEstimate<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ReciprocalEstimate(x, destination);
    }

    public static void MultiplyAddEstimate<T>(
        ReadOnlySpan<T> x,
        ReadOnlySpan<T> y,
        ReadOnlySpan<T> addend,
        in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == y.Length && y.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.MultiplyAddEstimate(x, y, addend, destination);
    }

    public static void MultiplyAddEstimate<T>(
        ReadOnlySpan<T> x,
        ReadOnlySpan<T> y,
        T addend,
        in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MultiplyAddEstimate(x, y, addend, destination);
    }

    public static void MultiplyAddEstimate<T>(
        ReadOnlySpan<T> x,
        T y,
        ReadOnlySpan<T> addend,
        in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == addend.Length && addend.Length == destination.Length);
        TensorPrimitives.MultiplyAddEstimate(x, y, addend, destination);
    }
}
