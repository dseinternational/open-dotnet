// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    // -------- MaxMagnitude --------

    public static T MaxMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MaxMagnitude(x);
    }

    public static T MaxMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MaxMagnitude(x.AsSpan());
    }

    public static void MaxMagnitude<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxMagnitude(x, y, destination);
    }

    public static void MaxMagnitude<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MaxMagnitude(x, y, destination);
    }

    // -------- MinMagnitude --------

    public static T MinMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MinMagnitude(x);
    }

    public static T MinMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MinMagnitude(x.AsSpan());
    }

    public static void MinMagnitude<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinMagnitude(x, y, destination);
    }

    public static void MinMagnitude<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MinMagnitude(x, y, destination);
    }

    // -------- MaxNumber / MinNumber (NaN-aware: return the number, not NaN) --------

    public static T MaxNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.MaxNumber(x);
    }

    public static T MaxNumber<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MaxNumber(x.AsSpan());
    }

    public static void MaxNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxNumber(x, y, destination);
    }

    public static void MaxNumber<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MaxNumber(x, y, destination);
    }

    public static T MinNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.MinNumber(x);
    }

    public static T MinNumber<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MinNumber(x.AsSpan());
    }

    public static void MinNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinNumber(x, y, destination);
    }

    public static void MinNumber<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MinNumber(x, y, destination);
    }

    // -------- MaxMagnitudeNumber / MinMagnitudeNumber --------

    public static T MaxMagnitudeNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MaxMagnitudeNumber(x);
    }

    public static T MinMagnitudeNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MinMagnitudeNumber(x);
    }

    public static void MaxMagnitudeNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxMagnitudeNumber(x, y, destination);
    }

    public static void MinMagnitudeNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinMagnitudeNumber(x, y, destination);
    }

    // -------- IndexOfMaxMagnitude / IndexOfMinMagnitude --------

    public static int IndexOfMaxMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.IndexOfMaxMagnitude(x);
    }

    public static int IndexOfMaxMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IndexOfMaxMagnitude(x.AsSpan());
    }

    public static int IndexOfMinMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.IndexOfMinMagnitude(x);
    }

    public static int IndexOfMinMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IndexOfMinMagnitude(x.AsSpan());
    }
}
