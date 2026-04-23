// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void SinPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.SinPi(x, destination);
    }

    public static void CosPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.CosPi(x, destination);
    }

    public static void TanPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.TanPi(x, destination);
    }

    public static void SinCosPi<T>(ReadOnlySpan<T> x, in Span<T> sinDestination, in Span<T> cosDestination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == sinDestination.Length && sinDestination.Length == cosDestination.Length);
        TensorPrimitives.SinCosPi(x, sinDestination, cosDestination);
    }

    public static void AsinPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.AsinPi(x, destination);
    }

    public static void AcosPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.AcosPi(x, destination);
    }

    public static void AtanPi<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.AtanPi(x, destination);
    }

    public static void Atan2Pi<T>(ReadOnlySpan<T> y, ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == x.Length && x.Length == destination.Length);
        TensorPrimitives.Atan2Pi(y, x, destination);
    }

    public static void Atan2Pi<T>(ReadOnlySpan<T> y, T x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Atan2Pi(y, x, destination);
    }

    public static void Atan2Pi<T>(T y, ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Atan2Pi(y, x, destination);
    }
}
