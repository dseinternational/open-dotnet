// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void CopySign<T>(this IReadOnlySeries<T> x, T sign, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.CopySign(x.Vector.AsSpan(), sign, destination);
    }

    public static void CopySign<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> sign, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        VectorPrimitives.CopySign(x.Vector.AsSpan(), sign.Vector.AsSpan(), destination);
    }

    public static Series<T> CopySign<T>(this IReadOnlySeries<T> x, T sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.CopySign(sign);
    }

    public static Series<T> CopySign<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> sign)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(sign);
        return x.Vector.CopySign(sign.Vector);
    }

    public static void Sign<T>(this IReadOnlySeries<T> x, Span<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Sign(x.Vector.AsSpan(), destination);
    }

    public static void Sign<T>(this IReadOnlySeries<T> x, ISeries<int> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sign(x, destination.AsSpan());
    }

    public static Series<int> Sign<T>(this IReadOnlySeries<T> x)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.Sign();
    }
}
