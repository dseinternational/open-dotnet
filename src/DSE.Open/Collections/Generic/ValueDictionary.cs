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
    public static readonly ValueDictionary<TKey, TValue> Empty = new();

    private readonly Dictionary<TKey, TValue> _inner;

    public ValueDictionary() : this(Enumerable.Empty<KeyValuePair<TKey, TValue>>())
    {
    }

    public ValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        _inner = new Dictionary<TKey, TValue>(collection);
    }

    public ValueDictionary(IDictionary<TKey, TValue> source)
    {
        _inner = new Dictionary<TKey, TValue>(source);
    }

    public TValue this[TKey key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }

    public ICollection<TKey> Keys => _inner.Keys;

    public ICollection<TValue> Values => _inner.Values;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    public int Count => _inner.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value) => _inner.Add(key, value);

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        => ((IDictionary<TKey, TValue>)_inner).Add(item);

    public void Clear() => _inner.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => _inner.Contains(item);

    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);

    public override bool Equals(object? obj) => Equals(this, obj as ValueDictionary<TKey, TValue>);

    public bool Equals(ValueDictionary<TKey, TValue>? other)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>)this, other);

    public static bool operator ==(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>?)left, right);

    public static bool operator !=(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        => !DictionaryEqualityComparer<TKey, TValue>.Default.Equals((IDictionary<TKey, TValue>?)left, right);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _inner.GetEnumerator();

    public override int GetHashCode()
        => DictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode((IDictionary<TKey, TValue>)this);

    public bool Remove(TKey key) => _inner.Remove(key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Remove(item);

    public override string ToString() => DictionaryWriter.WriteToString(this)!; // Only null if `this` is null, which it isn't

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _inner.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();

    public ReadOnlyValueDictionary<TKey, TValue> AsReadOnly() => new(this);
}
