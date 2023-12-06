// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class DictionaryEqualityComparer<TKey, TValue>
    : IEqualityComparer<IReadOnlyDictionary<TKey, TValue>>,
      IEqualityComparer<IDictionary<TKey, TValue>>
    where TKey : notnull
{
    public static readonly DictionaryEqualityComparer<TKey, TValue> Default = new();

    private DictionaryEqualityComparer()
    {
        ValueComparer = EqualityComparer<TValue>.Default;
    }

    public IEqualityComparer<TValue> ValueComparer { get; }

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

    public int GetHashCode(ReadOnlyValueDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var hashCode = -7291863;

        foreach (var kvp in obj)
        {
            hashCode = HashCode.Combine(hashCode, kvp.Key);

            if (kvp.Value is not null)
            {
                hashCode = HashCode.Combine(hashCode, kvp.Value);
            }
        }

        return hashCode;
    }

    public int GetHashCode(IReadOnlyDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var hashCode = -7291863;

        foreach (var kvp in obj)
        {
            hashCode = HashCode.Combine(hashCode, kvp.Key);

            if (kvp.Value is not null)
            {
                hashCode = HashCode.Combine(hashCode, kvp.Value);
            }
        }

        return hashCode;
    }

    public int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var hashCode = -7291863;

        foreach (var kvp in obj)
        {
            hashCode = HashCode.Combine(hashCode, kvp.Key);

            if (kvp.Value is not null)
            {
                hashCode = HashCode.Combine(hashCode, kvp.Value);
            }
        }

        return hashCode;
    }
}
