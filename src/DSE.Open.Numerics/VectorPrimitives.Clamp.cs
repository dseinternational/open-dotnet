// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>
    public static void Clamp<T>(ReadOnlySpan<T> x, T min, T max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> min, ReadOnlySpan<T> max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == min.Length && min.Length == max.Length && max.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> min, T max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == min.Length && min.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(ReadOnlySpan<T> x, T min, ReadOnlySpan<T> max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == max.Length && max.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(T x, ReadOnlySpan<T> min, ReadOnlySpan<T> max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(min.Length == max.Length && max.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(T x, ReadOnlySpan<T> min, T max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(min.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(T x, T min, ReadOnlySpan<T> max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(max.Length == destination.Length);
        TensorPrimitives.Clamp(x, min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(this IReadOnlyVector<T> x, T min, T max, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Clamp(x.AsSpan(), min, max, destination);
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static void Clamp<T>(this IReadOnlyVector<T> x, T min, T max, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Clamp(x, min, max, destination.AsSpan());
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c>.</summary>

    public static Vector<T> Clamp<T>(this IReadOnlyVector<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Clamp(x, min, max, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise clamp into <c>[low, high]</c> (in place).</summary>

    public static void ClampInPlace<T>(this IVector<T> x, T min, T max)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Clamp(x.AsSpan(), min, max, x.AsSpan());
    }
}
