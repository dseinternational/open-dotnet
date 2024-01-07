// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open;

public partial class MemoryExtensions
{
    public static TSource Min<TSource>(this ReadOnlySpan<TSource> values) where TSource : INumber<TSource>
    {
        Guard.IsNotEmpty(values);

        var result = values[0];

        for (var i = 1; i < values.Length; i++)
        {
            result = TSource.Min(result, values[i]);
        }

        return result;
    }

    public static TSource MinBy<TSource, TKey>(this ReadOnlySpan<TSource> values, Func<TSource, TKey> keySelector)
        where TKey : INumber<TKey>
    {
        Guard.IsNotEmpty(values);
        ArgumentNullException.ThrowIfNull(keySelector);

        var result = (value: values[0], key: keySelector(values[0]));

        for (var i = 1; i < values.Length; i++)
        {
            var curr = (values[i], keySelector(values[i]));

            if (curr.Item2 < result.Item2)
            {
                result = curr;
            }
        }

        return result.value;
    }

    public static TSource Max<TSource>(this ReadOnlySpan<TSource> values) where TSource : INumber<TSource>
    {
        Guard.IsNotEmpty(values);

        var result = values[0];

        for (var i = 1; i < values.Length; i++)
        {
            result = TSource.Max(result, values[i]);
        }

        return result;
    }

    public static TSource MaxBy<TSource, TKey>(this ReadOnlySpan<TSource> values, Func<TSource, TKey> keySelector)
        where TKey : INumber<TKey>
    {
        Guard.IsNotEmpty(values);
        ArgumentNullException.ThrowIfNull(keySelector);

        var result = (value: values[0], key: keySelector(values[0]));

        for (var i = 1; i < values.Length; i++)
        {
            var curr = (values[i], keySelector(values[i]));

            if (curr.Item2 > result.Item2)
            {
                result = curr;
            }
        }

        return result.value;
    }
}
