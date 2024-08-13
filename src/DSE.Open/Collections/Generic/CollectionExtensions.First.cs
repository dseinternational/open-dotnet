// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public static partial class CollectionExtensionsFirst
{
    /// <summary>
    /// Returns the first element of a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T First<T>(this IList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            ThrowHelper.ThrowInvalidOperationException("The list is empty.");
            return default!; // unreachable
        }

        return collection[0];
    }

    /// <summary>
    /// Returns the first element of a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T First<T>(this IReadOnlyList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            ThrowHelper.ThrowInvalidOperationException("The list is empty.");
            return default!; // unreachable
        }

        return collection[0];
    }

    /// <summary>
    /// Returns the first element of a list, or a default value if no element is found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T FirstOrDefault<T>(this IList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            return default!;
        }

        return collection[0];
    }

    /// <summary>
    /// Returns the first element of a list, or a default value if no element is found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T FirstOrDefault<T>(this IReadOnlyList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            return default!;
        }

        return collection[0];
    }
}
