// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise <c>x / y</c>.</summary>
    public static void Divide<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Divide(x.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Divide(x, y.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var destination = CreateUninitializedArray<T>(x.Length);
        Divide(x, y, destination);
        return Vector.Create(destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Divide(x, y.AsSpan());
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlyVector<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Divide(x.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static void Divide<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x / y</c>.</summary>

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = CreateUninitializedArray<T>(x.Length);
        Divide(x, y, destination);
        return Vector.Create(destination);
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        DivideInPlace(x, y.AsSpan());
    }

    /// <summary>Element-wise <c>x /= y</c> in place.</summary>

    public static void DivideInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }
}
