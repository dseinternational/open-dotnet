// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Collections.Generic;

public static class DictionaryExtensions
{
    public static ImmutableDictionary<TKey, TValue> AsImmutable<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        return ImmutableDictionary.CreateRange(dictionary);
    }

    public static ReadOnlyValueDictionary<TKey, TValue> AsReadOnlyValueDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        where TKey : IEquatable<TKey>
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        return new(dictionary);
    }

    public static ValueDictionary<TKey, TValue> AsValueDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        where TKey : IEquatable<TKey>
    {
        ArgumentNullException.ThrowIfNull(dictionary);
#pragma warning disable IDE0306 // Simplify collection initialization
        return new(dictionary);
#pragma warning restore IDE0306 // Simplify collection initialization
    }

    public static void AddOrSet<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        KeyValuePair<TKey, TValue> keyValuePair)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        dictionary.AddOrSet(keyValuePair.Key, keyValuePair.Value);
    }

    public static void AddOrSet<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        dictionary[key] = value;
    }

    public static void AddOrSetRange<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(keyValuePairs);

        foreach (var item in keyValuePairs)
        {
            AddOrSet(dictionary, item.Key, item.Value);
        }
    }

    /// <summary>
    /// Gets the items with the specified keys.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">A key was specified that did not match the key of
    /// any element in the collection.</exception>
    public static IEnumerable<TItem> GetAll<TKey, TItem>(
        this IDictionary<TKey, TItem> dictionary,
        IEnumerable<TKey> keys) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(keys);

        foreach (var k in keys)
        {
            yield return dictionary[k];
        }
    }
}
