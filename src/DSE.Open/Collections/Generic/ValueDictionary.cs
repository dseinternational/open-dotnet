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
      IEquatable<ValueDictionary<TKey, TValue>>
    where TKey : notnull, IEquatable<TKey>
{
    public static readonly ValueDictionary<TKey, TValue> Empty = new();

    private readonly IDictionary<TKey, TValue> _inner;

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

    public int Count => _inner.Count;

    public bool IsReadOnly => _inner.IsReadOnly;

    public void Add(TKey key, TValue value) => _inner.Add(key, value);

    public void Add(KeyValuePair<TKey, TValue> item) => _inner.Add(item);

    public void Clear() => _inner.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => _inner.Contains(item);

    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _inner.CopyTo(array, arrayIndex);

    public override bool Equals(object? obj) => Equals(this, obj as ValueDictionary<TKey, TValue>);

    public bool Equals(ValueDictionary<TKey, TValue>? other)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals(this, other);

    public static bool operator ==(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);

    public static bool operator !=(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        => !DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _inner.GetEnumerator();

    public override int GetHashCode()
        => DictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode(this);

    public bool Remove(TKey key) => _inner.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item)
        => _inner.Remove(item);

    public override string ToString() => DictionaryWriter.WriteToString(this)!; // Only null if `this` is null, which it isn't

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _inner.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();
}
