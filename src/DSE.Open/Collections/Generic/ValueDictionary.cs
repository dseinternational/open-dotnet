// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A dictionary where equality is based on the equality of its contents.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public sealed class ValueDictionary<TKey, TValue>
    : IDictionary<TKey, TValue>,
      IReadOnlyDictionary<TKey, TValue>,
      IEquatable<ValueDictionary<TKey, TValue>>
    where TKey : notnull
{
    /// <summary>
    /// An empty <see cref="ValueDictionary{TKey, TValue}"/>.
    /// </summary>
    public static readonly ValueDictionary<TKey, TValue> Empty = [];

    private readonly Dictionary<TKey, TValue> _inner;

    /// <summary>
    /// Initializes a new, empty <see cref="ValueDictionary{TKey, TValue}"/>.
    /// </summary>
    public ValueDictionary() : this([])
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ValueDictionary{TKey, TValue}"/> containing the entries of <paramref name="collection"/>.
    /// </summary>
    public ValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        _inner = new(collection);
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    /// <summary>
    /// Initializes a new <see cref="ValueDictionary{TKey, TValue}"/> containing the entries of <paramref name="source"/>.
    /// </summary>
    public ValueDictionary(IDictionary<TKey, TValue> source)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        _inner = new(source);
#pragma warning restore IDE0028 // Simplify collection initialization

    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }

    /// <inheritdoc/>
    public ICollection<TKey> Keys => _inner.Keys;

    /// <inheritdoc/>
    public ICollection<TValue> Values => _inner.Values;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    /// <inheritdoc/>
    public int Count => _inner.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        _inner.Add(key, value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey, TValue>)_inner).Add(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _inner.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _inner.Contains(item);
    }

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
    {
        return _inner.ContainsKey(key);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(this, obj as ValueDictionary<TKey, TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(ValueDictionary<TKey, TValue>? other)
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>)this, other);
    }

    /// <summary>
    /// Gets an object that can be used to synchronize access to the dictionary.
    /// </summary>
    public Lock SyncRoot { get; } = new();

    /// <summary>
    /// Indicates whether the two dictionaries contain the same set of entries.
    /// </summary>
    public static bool operator ==(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>?)left, right);
    }

    /// <summary>
    /// Indicates whether the two dictionaries do not contain the same set of entries.
    /// </summary>
    public static bool operator !=(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
    {
        return !DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>?)left, right);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode((IDictionary<TKey, TValue>)this);
    }

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        return _inner.Remove(key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Remove(item);
    }

    /// <summary>
    /// Returns a string representation of the dictionary produced by <see cref="CollectionWriter"/>.
    /// </summary>
    public override string ToString()
    {
        return CollectionWriter.WriteToString(this)!;
        // Only null if `this` is null, which it isn't
    }

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _inner.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_inner).GetEnumerator();
    }

    /// <summary>
    /// Returns a <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> view over this dictionary.
    /// </summary>
    public ReadOnlyValueDictionary<TKey, TValue> AsReadOnly()
    {
        return new(this);
    }
}
