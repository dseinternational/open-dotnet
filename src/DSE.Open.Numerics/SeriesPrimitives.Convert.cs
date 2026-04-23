// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void ConvertChecked<TFrom, TTo>(this IReadOnlySeries<TFrom> source, Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertChecked<TFrom, TTo>(source.Vector.AsSpan(), destination);
    }

    public static void ConvertSaturating<TFrom, TTo>(this IReadOnlySeries<TFrom> source, Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertSaturating<TFrom, TTo>(source.Vector.AsSpan(), destination);
    }

    public static void ConvertTruncating<TFrom, TTo>(this IReadOnlySeries<TFrom> source, Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertTruncating<TFrom, TTo>(source.Vector.AsSpan(), destination);
    }

    public static void ConvertToInteger<TFrom, TTo>(this IReadOnlySeries<TFrom> source, Span<TTo> destination)
        where TFrom : struct, IFloatingPoint<TFrom>
        where TTo : struct, IBinaryInteger<TTo>
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertToInteger<TFrom, TTo>(source.Vector.AsSpan(), destination);
    }

    public static void ConvertToIntegerNative<TFrom, TTo>(this IReadOnlySeries<TFrom> source, Span<TTo> destination)
        where TFrom : struct, IFloatingPoint<TFrom>
        where TTo : struct, IBinaryInteger<TTo>
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertToIntegerNative<TFrom, TTo>(source.Vector.AsSpan(), destination);
    }

    public static void ConvertToHalf(this IReadOnlySeries<float> source, Span<Half> destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertToHalf(source.Vector.AsSpan(), destination);
    }

    public static void ConvertToSingle(this IReadOnlySeries<Half> source, Span<float> destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        VectorPrimitives.ConvertToSingle(source.Vector.AsSpan(), destination);
    }
}
