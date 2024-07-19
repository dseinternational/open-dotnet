// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

public class ReadOnlyKeyedCollection<TKey, TItem> : IReadOnlyKeyedCollection<TKey, TItem>
    where TKey : notnull
{
    private readonly KeyedCollection<TKey, TItem> _innerCollection;

    public ReadOnlyKeyedCollection(KeyedCollection<TKey, TItem> collection)
    {
        _innerCollection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    public TItem this[int index]
    {
        get => index < 0 ? throw new ArgumentOutOfRangeException(nameof(index)) : _innerCollection[index];
        set => throw new NotSupportedException();
    }

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

    public bool Contains(TItem item)
    {
        return _innerCollection.Contains(item);
    }

    void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
    {
        _innerCollection.CopyTo(array, arrayIndex);
    }

    public bool IsFixedSize => true;

    public bool IsReadOnly => true;

    bool ICollection<TItem>.Remove(TItem item)
    {
        throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<TItem> GetEnumerator()
    {
        return _innerCollection.GetEnumerator();
    }

    public int Count => _innerCollection.Count;

    public bool IsSynchronized => ((ICollection)_innerCollection).IsSynchronized;

    public object SyncRoot => ((ICollection)_innerCollection).SyncRoot;

    object? IList.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    public bool Contains(TKey key)
    {
        return _innerCollection.Contains(key);
    }

    public int IndexOf(TItem item)
    {
        return _innerCollection.IndexOf(item);
    }

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

    protected virtual void CopyTo(Array array, int index)
    {
        ((ICollection)_innerCollection).CopyTo(array, index);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        CopyTo(array, index);
    }
}
