// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void Negate<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Negate(x.Vector.AsSpan(), destination);
    }

    public static void Negate<T>(this IReadOnlySeries<T> x, ISeries<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Negate(x, destination.AsSpan());
    }

    public static Series<T> Negate<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Negate(), x);
    }

    public static void NegateInPlace<T>(this ISeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Negate(x.AsSpan(), x.AsSpan());
    }
}
