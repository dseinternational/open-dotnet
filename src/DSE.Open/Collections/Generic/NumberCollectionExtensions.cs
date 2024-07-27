// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Collections.Generic;

public static class NumberCollectionExtensions
{
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
