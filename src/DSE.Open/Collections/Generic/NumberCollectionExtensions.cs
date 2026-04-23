// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Lazy conversions over sequences of <see cref="INumberBase{TSelf}"/> values.
/// </summary>
public static class NumberCollectionExtensions
{
    /// <summary>
    /// Projects each element to <typeparamref name="TTo"/> using <see cref="INumberBase{TSelf}.CreateChecked{TOther}"/>,
    /// throwing <see cref="OverflowException"/> when a value is not representable in the target type.
    /// </summary>
    /// <typeparam name="TFrom">The source number type.</typeparam>
    /// <typeparam name="TTo">The target number type.</typeparam>
    /// <param name="collection">The source sequence.</param>
    /// <returns>A lazily-evaluated sequence of converted values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="OverflowException">A value is not representable in <typeparamref name="TTo"/>.</exception>
    public static IEnumerable<TTo> ConvertChecked<TFrom, TTo>(this IEnumerable<TFrom> collection)
        where TFrom : INumberBase<TFrom>
        where TTo : INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(collection);

        foreach (var value in collection)
        {
            yield return TTo.CreateChecked(value);
        }
    }

    /// <summary>
    /// Projects each element to <typeparamref name="TTo"/> using <see cref="INumberBase{TSelf}.CreateSaturating{TOther}"/>.
    /// Values that exceed the target range are clamped to <c>MinValue</c> or <c>MaxValue</c>.
    /// </summary>
    /// <typeparam name="TFrom">The source number type.</typeparam>
    /// <typeparam name="TTo">The target number type.</typeparam>
    /// <param name="collection">The source sequence.</param>
    /// <returns>A lazily-evaluated sequence of converted values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static IEnumerable<TTo> ConvertSaturating<TFrom, TTo>(this IEnumerable<TFrom> collection)
        where TFrom : INumberBase<TFrom>
        where TTo : INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(collection);

        foreach (var value in collection)
        {
            yield return TTo.CreateSaturating(value);
        }
    }

    /// <summary>
    /// Projects each element to <typeparamref name="TTo"/> using <see cref="INumberBase{TSelf}.CreateTruncating{TOther}"/>,
    /// truncating towards zero for fractional sources and wrapping modulo <c>2^n</c> for oversized integer sources.
    /// </summary>
    /// <typeparam name="TFrom">The source number type.</typeparam>
    /// <typeparam name="TTo">The target number type.</typeparam>
    /// <param name="collection">The source sequence.</param>
    /// <returns>A lazily-evaluated sequence of converted values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static IEnumerable<TTo> ConvertTruncating<TFrom, TTo>(this IEnumerable<TFrom> collection)
        where TFrom : INumberBase<TFrom>
        where TTo : INumberBase<TTo>
    {
        ArgumentNullException.ThrowIfNull(collection);

        foreach (var value in collection)
        {
            yield return TTo.CreateTruncating(value);
        }
    }
}
