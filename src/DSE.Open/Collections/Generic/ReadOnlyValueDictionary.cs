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
/// <remarks><see cref="IDictionary{TKey,TValue}"/> is implemented explicitly to support deserialization.</remarks>
[SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Required")]
public class ReadOnlyValueDictionary<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>,
      IDictionary<TKey, TValue>,
      IEquatable<ReadOnlyValueDictionary<TKey, TValue>>
    where TKey : notnull
{
    /// <summary>
    /// An empty <see cref="ReadOnlyValueDictionary{TKey, TValue}"/>.
    /// </summary>
    public static readonly ReadOnlyValueDictionary<TKey, TValue> Empty = new();

    private readonly Dictionary<TKey, TValue> _inner;

    /// <summary>
    /// Initializes a new, empty <see cref="ReadOnlyValueDictionary{TKey, TValue}"/>.
    /// </summary>
    public ReadOnlyValueDictionary() : this([])
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of <paramref name="collection"/>.
    /// </summary>
    public ReadOnlyValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer = null)
    {
        _inner = collection is ReadOnlyValueDictionary<TKey, TValue> other
            && (comparer is null || other._inner.Comparer == comparer)
            ? other._inner
            : new(collection, comparer);
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of <paramref name="source"/>.
    /// </summary>
    public ReadOnlyValueDictionary(IDictionary<TKey, TValue> source, IEqualityComparer<TKey>? comparer = null)
    {
        _inner = source is ReadOnlyValueDictionary<TKey, TValue> other
            && (comparer is null || other._inner.Comparer == comparer)
            ? other._inner
            : new(source, comparer);
    }

    /// <inheritdoc/>
    public TValue this[TKey key] => _inner[key];

    TValue IDictionary<TKey, TValue>.this[TKey key] { get => _inner[key]; set => _inner[key] = value; }

    /// <inheritdoc/>
    public IEnumerable<TKey> Keys => _inner.Keys;

    /// <inheritdoc/>
    public IEnumerable<TValue> Values => _inner.Values;

    /// <inheritdoc/>
    public int Count => _inner.Count;

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => _inner.Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => _inner.Values;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).IsReadOnly;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
    {
        return _inner.ContainsKey(key);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyValueDictionary<TKey, TValue> d && Equals(this, d);
    }

    /// <inheritdoc/>
    public virtual bool Equals(ReadOnlyValueDictionary<TKey, TValue>? other)
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.Equals(this, other);
    }

    /// <summary>
    /// Indicates whether the two dictionaries contain the same set of entries.
    /// </summary>
    public static bool operator ==(ReadOnlyValueDictionary<TKey, TValue>? left, ReadOnlyValueDictionary<TKey, TValue>? right)
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);
    }

    /// <summary>
    /// Indicates whether the two dictionaries do not contain the same set of entries.
    /// </summary>
    public static bool operator !=(ReadOnlyValueDictionary<TKey, TValue>? left, ReadOnlyValueDictionary<TKey, TValue>? right)
    {
        return !DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return DictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode(this);
    }

    /// <summary>
    /// Returns a string representation of the dictionary produced by <see cref="CollectionWriter"/>.
    /// </summary>
    public override string ToString()
    {
        return CollectionWriter.WriteToString(this);
    }

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _inner.TryGetValue(key, out value);
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
    {
        _inner.Add(key, value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Add(item);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
        _inner.Clear();
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return _inner.Contains(item);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_inner).GetEnumerator();
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
        return _inner.Remove(key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Remove(item);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of the specified array.
    /// </summary>
    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(KeyValuePair<TKey, TValue>[] value)
    {
        return new(value);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(Collection<KeyValuePair<TKey, TValue>> value)
    {
        return new(value);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(System.Collections.ObjectModel.Collection<KeyValuePair<TKey, TValue>> value)
    {
        return new(value);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueDictionary{TKey, TValue}"/> containing the entries of the specified dictionary.
    /// </summary>
    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(Dictionary<TKey, TValue> value)
    {
        return new(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
