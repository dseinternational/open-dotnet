// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Compares dictionaries for structural equality. Two dictionaries are considered equal
/// when they contain the same set of keys and the values under each key match according
/// to <see cref="ValueComparer"/>. Iteration order does not affect <see cref="Equals(IDictionary{TKey,TValue},IDictionary{TKey,TValue})"/>.
/// </summary>
/// <typeparam name="TKey">The dictionary key type.</typeparam>
/// <typeparam name="TValue">The dictionary value type.</typeparam>
public class DictionaryEqualityComparer<TKey, TValue>
    : IEqualityComparer<IReadOnlyDictionary<TKey, TValue>>,
      IEqualityComparer<IDictionary<TKey, TValue>>
    where TKey : notnull
{
    /// <summary>
    /// The singleton instance that uses <see cref="EqualityComparer{T}.Default"/> for values.
    /// </summary>
    public static readonly DictionaryEqualityComparer<TKey, TValue> Default = new();

    private DictionaryEqualityComparer()
    {
        ValueComparer = EqualityComparer<TValue>.Default;
    }

    /// <summary>
    /// The value equality comparer used when comparing entries under the same key.
    /// </summary>
    public IEqualityComparer<TValue> ValueComparer { get; }

    /// <summary>
    /// Determines whether two <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> instances contain the same key/value entries.
    /// </summary>
    public bool Equals(ReadOnlyValueDictionary<TKey, TValue>? x, ReadOnlyValueDictionary<TKey, TValue>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value2))
            {
                return false;
            }

            if (!ValueComparer.Equals(x[kvp.Key], value2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether two <see cref="ReadOnlySortedValueDictionary{TKey, TValue}"/> instances contain the same key/value entries.
    /// </summary>
    public bool Equals(ReadOnlySortedValueDictionary<TKey, TValue>? x, ReadOnlySortedValueDictionary<TKey, TValue>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value2))
            {
                return false;
            }

            if (!ValueComparer.Equals(x[kvp.Key], value2))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Equals(IReadOnlyDictionary<TKey, TValue>? x, IReadOnlyDictionary<TKey, TValue>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value2))
            {
                return false;
            }

            if (!ValueComparer.Equals(x[kvp.Key], value2))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Equals(IDictionary<TKey, TValue>? x, IDictionary<TKey, TValue>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value2))
            {
                return false;
            }

            if (!ValueComparer.Equals(x[kvp.Key], value2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns a hash code for the specified <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> based on its entries.
    /// </summary>
    public int GetHashCode(ReadOnlyValueDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return GetHashCodeCore(obj, obj.Count);
    }

    /// <summary>
    /// Returns a hash code for the specified <see cref="ReadOnlySortedValueDictionary{TKey, TValue}"/> based on its entries.
    /// </summary>
    public int GetHashCode(ReadOnlySortedValueDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return GetHashCodeCore(obj, obj.Count);
    }

    /// <inheritdoc/>
    public int GetHashCode(IReadOnlyDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return GetHashCodeCore(obj, obj.Count);
    }

    /// <inheritdoc/>
    public int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return GetHashCodeCore(obj, obj.Count);
    }

    private int GetHashCodeCore(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int count)
    {
        var hashCode = 0;

        foreach (var kvp in dictionary)
        {
            hashCode ^= HashCode.Combine(
                kvp.Key,
                kvp.Value is null ? 0 : ValueComparer.GetHashCode(kvp.Value));
        }

        return HashCode.Combine(count, hashCode);
    }
}
