// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A dictionary where equality is based on the equality of its contents.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <remarks><see cref="IDictionary{TKey,TValue}"/> is implemented explicitly to support deserialization.</remarks>
public sealed class ReadOnlyValueDictionary<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>,
      IDictionary<TKey, TValue>,
      IEquatable<ReadOnlyValueDictionary<TKey, TValue>>
    where TKey : notnull
{
    public static readonly ReadOnlyValueDictionary<TKey, TValue> Empty = new();

    private readonly Dictionary<TKey, TValue> _inner;

    public ReadOnlyValueDictionary() : this(Enumerable.Empty<KeyValuePair<TKey, TValue>>())
    {
    }

    public ReadOnlyValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        _inner = new Dictionary<TKey, TValue>(collection);
    }

    public ReadOnlyValueDictionary(IDictionary<TKey, TValue> source)
    {
        _inner = new Dictionary<TKey, TValue>(source);
    }

    public TValue this[TKey key] => _inner[key];

    TValue IDictionary<TKey, TValue>.this[TKey key] { get => _inner[key]; set => _inner[key] = value; }

    public IEnumerable<TKey> Keys => _inner.Keys;

    public IEnumerable<TValue> Values => _inner.Values;

    public int Count => _inner.Count;

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => _inner.Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => _inner.Values;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).IsReadOnly;

    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    public override bool Equals(object? obj)
        => obj is ReadOnlyValueDictionary<TKey, TValue> d && Equals(this, d);

    public bool Equals(ReadOnlyValueDictionary<TKey, TValue>? other)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals(this, other);

    public static bool operator ==(ReadOnlyValueDictionary<TKey, TValue>? left, ReadOnlyValueDictionary<TKey, TValue>? right)
        => DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);

    public static bool operator !=(ReadOnlyValueDictionary<TKey, TValue>? left, ReadOnlyValueDictionary<TKey, TValue>? right)
        => !DictionaryEqualityComparer<TKey, TValue>.Default.Equals(left, right);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _inner.GetEnumerator();

    public override int GetHashCode()
        => DictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode(this);

    public override string ToString()
        => DictionaryWriter.WriteToString(this)!; // Only null if `this` is null, which it isn't.

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _inner.TryGetValue(key, out value);

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => _inner.Add(key, value);

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Add(item);

    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => _inner.Clear();

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        => _inner.Contains(item);

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();

    bool IDictionary<TKey, TValue>.Remove(TKey key) => _inner.Remove(key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Remove(item);

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(KeyValuePair<TKey, TValue>[] value)
        => new(value);

    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(Collection<KeyValuePair<TKey, TValue>> value)
        => new(value);

    public static explicit operator ReadOnlyValueDictionary<TKey, TValue>(Dictionary<TKey, TValue> value)
        => new(value);

#pragma warning restore CA2225 // Operator overloads have named alternates
}
