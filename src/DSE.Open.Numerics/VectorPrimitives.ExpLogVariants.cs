// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Exp2<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Exp2(x, destination);
    }

    public static void Exp10<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Exp10(x, destination);
    }

    public static void ExpM1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.ExpM1(x, destination);
    }

    public static void Exp2M1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Exp2M1(x, destination);
    }

    public static void Exp10M1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Exp10M1(x, destination);
    }

    public static void LogP1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.LogP1(x, destination);
    }

    public static void Log2P1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log2P1(x, destination);
    }

    public static void Log10P1<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log10P1(x, destination);
    }
}
