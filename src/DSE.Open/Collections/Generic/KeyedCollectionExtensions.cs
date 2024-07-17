// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public static class KeyedCollectionExtensions
{
    /// <summary>
    /// Gets the items with the specified keys.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="collection"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">A key was specified that did not match the key of any element in the collection.</exception>
    public static IEnumerable<TItem> GetAll<TKey, TItem>(this IKeyedCollection<TKey, TItem> collection, IEnumerable<TKey> keys)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keys);

        foreach (var k in keys)
        {
            yield return collection[k];
        }
    }
}
