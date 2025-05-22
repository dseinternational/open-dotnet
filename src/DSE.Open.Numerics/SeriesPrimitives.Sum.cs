// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>
    /// Computes the sum of the elements in the series of numbers using the
    /// <see cref="IAdditionOperators{TSelf, TOther, TResult}"/> implementation provided
    /// by the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="series">The series to sum.</param>
    /// <returns>The result of adding all elements in <paramref name="series"/>, zero if
    /// <paramref name="series"/> is empty, or NaN if any element is equal to
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>.</returns>
    /// <remarks>
    /// ⚠️ If any of the elements in the series is equal to <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result is also NaN.
    /// <para>⚠️ Addition operations are unchecked and may overflow.</para>
    /// <para>⚠️ This method may call into the underlying C runtime or employ instructions specific
    /// to the current architecture. Exact results may differ between different operating systems
    /// or architectures.</para>
    /// </remarks>
    public static T Sum<T>(this IReadOnlySeries<T> series)
        where T : IEquatable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.Sum();
    }

    public static TResult SumChecked<T, TResult>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.SumChecked<T, TResult>();
    }

    public static T SumChecked<T>(this IReadOnlySeries<T> series)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.SumChecked();
    }

    public static TResult SumChecked<T, TResult>(
        this IReadOnlySeries<T> series,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Vector.SumChecked<T, TResult>(summation);
    }
}
