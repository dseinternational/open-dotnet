// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Multiply<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Multiply(x.AsSpan(), y, destination);
    }

    public static void Multiply<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Multiply(x, y.AsSpan(), destination);
    }

    public static void Multiply<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Multiply(x, y, destination.AsSpan());
    }

    public static Vector<T> Multiply<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var destination = Vector.Create<T>(x.Length);
        Multiply(x, y, destination.AsSpan());
        return destination;
    }

    public static Vector<T> Multiply<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Multiply(x, y.AsSpan());
    }

    public static void Multiply<T>(this IReadOnlyVector<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Multiply(x.AsSpan(), y, destination);
    }

    public static void Multiply<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Multiply(x, y, destination.AsSpan());
    }

    public static Vector<T> Multiply<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Multiply(x, y, destination.AsSpan());
        return destination;
    }

    public static void MultiplyInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Multiply(x, y, x.AsSpan());
    }

    public static void MultiplyInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        MultiplyInPlace(x, y.AsSpan());
    }

    public static void MultiplyInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Multiply(x, y, x.AsSpan());
    }
}
