// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A read-only wrapper around a <see cref="KeyedCollection{TKey, TItem}"/> that exposes both index- and key-based lookup.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TItem">The item type.</typeparam>
public class ReadOnlyKeyedCollection<TKey, TItem> : IReadOnlyKeyedCollection<TKey, TItem>
    where TKey : notnull
{
    private readonly KeyedCollection<TKey, TItem> _innerCollection;

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyKeyedCollection{TKey, TItem}"/> wrapping the specified keyed collection.
    /// </summary>
    public ReadOnlyKeyedCollection(KeyedCollection<TKey, TItem> collection)
    {
        _innerCollection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    /// <inheritdoc/>
    public TItem this[int index]
    {
        get => index < 0 ? throw new ArgumentOutOfRangeException(nameof(index)) : _innerCollection[index];
        set => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public TItem this[TKey key]
    {
        get => _innerCollection[key];
        set => throw new NotSupportedException();
    }

    void ICollection<TItem>.Add(TItem item)
    {
        throw new NotSupportedException();
    }

    void ICollection<TItem>.Clear()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public bool Contains(TItem item)
    {
        return _innerCollection.Contains(item);
    }

    void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
    {
        _innerCollection.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Gets a value indicating whether the collection has a fixed size. Always <see langword="true"/>.
    /// </summary>
    public bool IsFixedSize => true;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only. Always <see langword="true"/>.
    /// </summary>
    public bool IsReadOnly => true;

    bool ICollection<TItem>.Remove(TItem item)
    {
        throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc/>
    public IEnumerator<TItem> GetEnumerator()
    {
        return _innerCollection.GetEnumerator();
    }

    /// <inheritdoc/>
    public int Count => _innerCollection.Count;

    /// <summary>
    /// Gets a value indicating whether access to the underlying collection is synchronized (thread-safe).
    /// </summary>
    public bool IsSynchronized => ((ICollection)_innerCollection).IsSynchronized;

    /// <summary>
    /// Gets an object that can be used to synchronize access to the underlying collection.
    /// </summary>
    public object SyncRoot => ((ICollection)_innerCollection).SyncRoot;

    object? IList.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Determines whether the underlying keyed collection contains an item with the specified key.
    /// </summary>
    public bool Contains(TKey key)
    {
        return _innerCollection.Contains(key);
    }

    /// <inheritdoc/>
    public int IndexOf(TItem item)
    {
        return _innerCollection.IndexOf(item);
    }

    /// <summary>
    /// Clears the underlying keyed collection.
    /// </summary>
    public void Clear()
    {
        _innerCollection.Clear();
    }

    void IList<TItem>.Insert(int index, TItem item)
    {
        throw new NotSupportedException();
    }

    void IList<TItem>.RemoveAt(int index)
    {
        throw new NotSupportedException();
    }

    int IList.Add(object? value)
    {
        throw new NotSupportedException();
    }

    bool IList.Contains(object? value)
    {
        throw new NotSupportedException();
    }

    int IList.IndexOf(object? value)
    {
        throw new NotSupportedException();
    }

    void IList.Insert(int index, object? value)
    {
        throw new NotSupportedException();
    }

    void IList.Remove(object? value)
    {
        throw new NotSupportedException();
    }

    void IList.RemoveAt(int index)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Copies the elements of the underlying keyed collection to <paramref name="array"/>, starting at <paramref name="index"/>.
    /// </summary>
    protected virtual void CopyTo(Array array, int index)
    {
        ((ICollection)_innerCollection).CopyTo(array, index);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        CopyTo(array, index);
    }
}
