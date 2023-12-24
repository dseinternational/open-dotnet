// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Collections.Immutable;

public static class ImmutableDictionaryExtensions
{
    /// <summary>
    /// Returns a new immutable dictionary containing the items in the original, with the specified items
    /// also added or (if keys already present) set.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="original"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IImmutableDictionary<TKey, TValue> AddOrSetRange<TKey, TValue>(
        this IImmutableDictionary<TKey, TValue> original,
        IEnumerable<KeyValuePair<TKey, TValue>> items)
        where TKey : notnull
    {
        var existing = new Dictionary<TKey, TValue>();
        existing.AddRange(original);
        existing.AddOrSetRange(items);
        return existing.ToImmutableDictionary();
    }
}
