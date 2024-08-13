// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public static partial class CollectionExtensionsLast
{
    /// <summary>
    /// Returns the last element of a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T Last<T>(this IReadOnlyList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            ThrowHelper.ThrowInvalidOperationException("The list is empty.");
            return default!; // unreachable
        }

        return collection[collection.Count - 1];
    }

    /// <summary>
    /// Returns the last element of a list, or a default value if no element is found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T LastOrDefault<T>(this IReadOnlyList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            return default!;
        }

        return collection[collection.Count - 1];
    }
}
