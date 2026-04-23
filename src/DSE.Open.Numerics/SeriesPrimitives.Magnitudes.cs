// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    // -------- MaxMagnitude / MinMagnitude --------

    public static T MaxMagnitude<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MaxMagnitude(x.Vector.AsSpan());
    }

    public static void MaxMagnitude<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.MaxMagnitude(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    public static T MinMagnitude<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MinMagnitude(x.Vector.AsSpan());
    }

    public static void MinMagnitude<T>(
        this IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        VectorPrimitives.MinMagnitude(x.Vector.AsSpan(), y.Vector.AsSpan(), destination);
    }

    // -------- MaxNumber / MinNumber --------

    public static T MaxNumber<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MaxNumber(x.Vector.AsSpan());
    }

    public static T MinNumber<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MinNumber(x.Vector.AsSpan());
    }

    public static T MaxMagnitudeNumber<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MaxMagnitudeNumber(x.Vector.AsSpan());
    }

    public static T MinMagnitudeNumber<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.MinMagnitudeNumber(x.Vector.AsSpan());
    }

    // -------- IndexOfMaxMagnitude / IndexOfMinMagnitude --------

    public static int IndexOfMaxMagnitude<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.IndexOfMaxMagnitude(x.Vector.AsSpan());
    }

    public static int IndexOfMinMagnitude<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return VectorPrimitives.IndexOfMinMagnitude(x.Vector.AsSpan());
    }
}
