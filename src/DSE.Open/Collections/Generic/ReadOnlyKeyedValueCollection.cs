// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A collection of values that can be accessed by index or key.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class ReadOnlyKeyedValueCollection<TKey, TValue>
    : ICollection<TValue>,
      IEquatable<ReadOnlyKeyedValueCollection<TKey, TValue>>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;

    protected ReadOnlyKeyedValueCollection()
    {
        _dictionary = new Dictionary<TKey, TValue>();
    }

    protected ReadOnlyKeyedValueCollection(IEnumerable<TValue> list)
    {
        Guard.IsNotNull(list);

        _dictionary = list is ReadOnlyKeyedValueCollection<TKey, TValue> other
            ? other._dictionary
            : list.ToDictionary(GetKeyForItem);
    }

    public int Count => _dictionary.Count;

    public TValue this[TKey key] => _dictionary[key];

    public IReadOnlyCollection<TKey> Keys => _dictionary.Keys;

    protected abstract TKey GetKeyForItem(TValue item);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue item)
    {
        Guard.IsNotNull(key);
        return _dictionary.TryGetValue(key, out item!);
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<TValue>.IsReadOnly => false; // JsonSerialization

    void ICollection<TValue>.Add(TValue item) => _dictionary.Add(GetKeyForItem(item), item); // JsonSerialization

    void ICollection<TValue>.Clear() => _dictionary.Clear(); // JsonSerialization

    bool ICollection<TValue>.Remove(TValue item)
    {
        ThrowHelper.ThrowNotSupportedException("Cannot remove items from a read-only collection.");
        return false;
    }

#pragma warning restore CA1033 // Interface methods should be callable by child types

    public bool Contains(TValue item) => ContainsKey(GetKeyForItem(item));

    public bool ContainsKey(TKey key)
    {
        Guard.IsNotNull(key);
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(TValue[] array, int arrayIndex) => _dictionary.Values.CopyTo(array, arrayIndex);

    public bool Equals(ReadOnlyKeyedValueCollection<TKey, TValue>? other)
    {
        return other is not null
            && (ReferenceEquals(this, other)
            || (Count == other.Count && this.SequenceEqual(other)));
    }

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyKeyedValueCollection<TKey, TValue>);

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    public IEnumerator<TValue> GetEnumerator() => _dictionary.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary.Values).GetEnumerator();

    /// <summary>
    /// Creates a mutable copy of the dictionary.
    /// </summary>
    /// <returns></returns>
    public Dictionary<TKey, TValue> ToDictionary() => new(_dictionary);
}
