// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    // -------- MaxMagnitude --------

    /// <summary>Element-wise maximum by magnitude.</summary>

    public static T MaxMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MaxMagnitude(x);
    }

    /// <summary>Element-wise maximum by magnitude.</summary>

    public static T MaxMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MaxMagnitude(x.AsSpan());
    }

    /// <summary>Element-wise maximum by magnitude.</summary>

    public static void MaxMagnitude<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxMagnitude(x, y, destination);
    }

    /// <summary>Element-wise maximum by magnitude.</summary>

    public static void MaxMagnitude<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MaxMagnitude(x, y, destination);
    }

    // -------- MinMagnitude --------

    /// <summary>Element-wise minimum by magnitude.</summary>

    public static T MinMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MinMagnitude(x);
    }

    /// <summary>Element-wise minimum by magnitude.</summary>

    public static T MinMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MinMagnitude(x.AsSpan());
    }

    /// <summary>Element-wise minimum by magnitude.</summary>

    public static void MinMagnitude<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinMagnitude(x, y, destination);
    }

    /// <summary>Element-wise minimum by magnitude.</summary>

    public static void MinMagnitude<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MinMagnitude(x, y, destination);
    }

    // -------- MaxNumber / MinNumber (NaN-aware: return the number, not NaN) --------

    /// <summary>Element-wise maximum (NaN-aware).</summary>

    public static T MaxNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.MaxNumber(x);
    }

    /// <summary>Element-wise maximum (NaN-aware).</summary>

    public static T MaxNumber<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MaxNumber(x.AsSpan());
    }

    /// <summary>Element-wise maximum (NaN-aware).</summary>

    public static void MaxNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxNumber(x, y, destination);
    }

    /// <summary>Element-wise maximum (NaN-aware).</summary>

    public static void MaxNumber<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MaxNumber(x, y, destination);
    }

    /// <summary>Element-wise minimum (NaN-aware).</summary>

    public static T MinNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.MinNumber(x);
    }

    /// <summary>Element-wise minimum (NaN-aware).</summary>

    public static T MinNumber<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return MinNumber(x.AsSpan());
    }

    /// <summary>Element-wise minimum (NaN-aware).</summary>

    public static void MinNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinNumber(x, y, destination);
    }

    /// <summary>Element-wise minimum (NaN-aware).</summary>

    public static void MinNumber<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.MinNumber(x, y, destination);
    }

    // -------- MaxMagnitudeNumber / MinMagnitudeNumber --------

    /// <summary>Element-wise maximum by magnitude (NaN-aware).</summary>

    public static T MaxMagnitudeNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MaxMagnitudeNumber(x);
    }

    /// <summary>Element-wise minimum by magnitude (NaN-aware).</summary>

    public static T MinMagnitudeNumber<T>(ReadOnlySpan<T> x)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.MinMagnitudeNumber(x);
    }

    /// <summary>Element-wise maximum by magnitude (NaN-aware).</summary>

    public static void MaxMagnitudeNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MaxMagnitudeNumber(x, y, destination);
    }

    /// <summary>Element-wise minimum by magnitude (NaN-aware).</summary>

    public static void MinMagnitudeNumber<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.MinMagnitudeNumber(x, y, destination);
    }

    // -------- IndexOfMaxMagnitude / IndexOfMinMagnitude --------

    /// <summary>Returns the index of the element with the largest magnitude.</summary>

    public static int IndexOfMaxMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.IndexOfMaxMagnitude(x);
    }

    /// <summary>Returns the index of the element with the largest magnitude.</summary>

    public static int IndexOfMaxMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IndexOfMaxMagnitude(x.AsSpan());
    }

    /// <summary>Returns the index of the element with the smallest magnitude.</summary>

    public static int IndexOfMinMagnitude<T>(ReadOnlySpan<T> x)
        where T : struct, INumber<T>
    {
        return TensorPrimitives.IndexOfMinMagnitude(x);
    }

    /// <summary>Returns the index of the element with the smallest magnitude.</summary>

    public static int IndexOfMinMagnitude<T>(this IReadOnlyVector<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IndexOfMinMagnitude(x.AsSpan());
    }
}
