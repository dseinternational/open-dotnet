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
      IReadOnlyCollection<TValue>,
      IEquatable<ReadOnlyKeyedValueCollection<TKey, TValue>>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;

    /// <summary>
    /// Initializes a new, empty <see cref="ReadOnlyKeyedValueCollection{TKey, TValue}"/>.
    /// </summary>
    protected ReadOnlyKeyedValueCollection()
    {
        _dictionary = [];
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyKeyedValueCollection{TKey, TValue}"/> containing the elements of <paramref name="list"/>, keyed by <see cref="GetKeyForItem"/>.
    /// </summary>
    protected ReadOnlyKeyedValueCollection(IEnumerable<TValue> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        _dictionary = list is ReadOnlyKeyedValueCollection<TKey, TValue> other
            ? other._dictionary
            : list.ToDictionary(GetKeyForItem);
    }

    /// <inheritdoc/>
    public int Count => _dictionary.Count;

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    public TValue this[TKey key] => _dictionary[key];

    /// <summary>
    /// Gets a read-only collection of the keys for items in this collection.
    /// </summary>
    public IReadOnlyCollection<TKey> Keys => _dictionary.Keys;

    /// <summary>
    /// When implemented in a derived class, returns the key for the specified item.
    /// </summary>
    protected abstract TKey GetKeyForItem(TValue item);

    /// <summary>
    /// Attempts to get the item associated with the specified key.
    /// </summary>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue item)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _dictionary.TryGetValue(key, out item!);
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<TValue>.IsReadOnly => false; // JsonSerialization

    void ICollection<TValue>.Add(TValue item)
    {
        _dictionary.Add(GetKeyForItem(item), item); // JsonSerialization
    }

    void ICollection<TValue>.Clear()
    {
        _dictionary.Clear(); // JsonSerialization
    }

    bool ICollection<TValue>.Remove(TValue item)
    {
        ThrowHelper.ThrowNotSupportedException("Cannot remove items from a read-only collection.");
        return false;
    }

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <inheritdoc/>
    public bool Contains(TValue item)
    {
        return ContainsKey(GetKeyForItem(item));
    }

    /// <summary>
    /// Determines whether the collection contains an item with the specified key.
    /// </summary>
    public bool ContainsKey(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _dictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    public void CopyTo(TValue[] array, int arrayIndex)
    {
        _dictionary.Values.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public virtual bool Equals(ReadOnlyKeyedValueCollection<TKey, TValue>? other)
    {
        return other is not null
            && (ReferenceEquals(this, other)
                || (Count == other.Count && this.SequenceEqual(other)));
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ReadOnlyKeyedValueCollection<TKey, TValue>);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc/>
    public IEnumerator<TValue> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary.Values).GetEnumerator();
    }

    /// <summary>
    /// Creates a mutable copy of the dictionary.
    /// </summary>
    /// <returns></returns>
    public Dictionary<TKey, TValue> ToDictionary()
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new(_dictionary);
#pragma warning restore IDE0028 // Simplify collection initialization
    }
}
