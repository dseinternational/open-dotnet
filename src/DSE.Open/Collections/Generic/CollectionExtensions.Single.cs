// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public static partial class CollectionExtensions
{
    /// <summary>
    /// Returns the single element of a list, or throws if the list is empty or contains more than one element.
    /// </summary>
    public static TSource Single<TSource>(this IReadOnlyList<TSource> source)
    {
        var single = source.TryGetSingle(out var found);

        if (!found)
        {
            ThrowHelper.ThrowNoElementsException();
        }

        return single!;
    }

    /// <summary>
    /// Returns the single element of a list, the default value if the list is empty, or throws if it contains more than one element.
    /// </summary>
    public static TSource? SingleOrDefault<TSource>(this IReadOnlyList<TSource> list)
    {
        var single = list.TryGetSingle(out var found);
        return found ? single! : default;
    }

    private static TSource? TryGetSingle<TSource>(this IReadOnlyList<TSource> source, out bool found)
    {
        ArgumentNullException.ThrowIfNull(source);

        switch (source.Count)
        {
            case 0:
                found = false;
                return default;
            case 1:
                found = true;
                return source[0];
            default:
                break;
        }

        found = false;
        ThrowHelper.ThrowMoreThanOneElementException();
        return default;
    }
}

static file class ThrowHelper
{
    public static void ThrowMoreThanOneElementException()
    {
        throw new InvalidOperationException("The collection contains more than one element.");
    }

    public static void ThrowNoElementsException()
    {
        throw new InvalidOperationException("The collection contains no elements.");
    }
}
