// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using DSE.Open.Text;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
///     <see cref="ICollection{T}"/> is implemented explicitly to support deserialization and <see cref="IList"/>
///     is implemented to support certain data-binding scenarios.
/// </remarks>
[CollectionBuilder(typeof(ReadOnlyCollection), nameof(ReadOnlyCollection.Create))]
public class ReadOnlyCollection<T>
    : IReadOnlyList<T>,
      ICollection<T>,
      IList
{
    public static readonly ReadOnlyCollection<T> Empty = [];

    internal readonly List<T> _items;

    public ReadOnlyCollection()
    {
        _items = [];
    }

    public ReadOnlyCollection(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = new(items);
    }

    /// <summary>
    /// Unsafely creates a <see cref="ReadOnlyCollection{T}"/> using the provided list as it's backing store.
    /// This should only be done when the caller is the only holder of the list, and does not mutate it after constructing this collection.
    /// </summary>
    internal ReadOnlyCollection(List<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        _items = collection;
    }

    public T this[int index] => _items[index];

    public int BinarySearch(T item)
    {
        return BinarySearch(0, Count, item, null);
    }

    public int BinarySearch(T item, IComparer<T>? comparer)
    {
        return BinarySearch(0, Count, item, comparer);
    }

    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
        return _items.BinarySearch(index, count, item, comparer);
    }

    public int Count => _items.Count;

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool IList.IsFixedSize => true;

    bool IList.IsReadOnly => true;

    bool ICollection.IsSynchronized => true;

    object ICollection.SyncRoot => ((IList)_items).SyncRoot;

    bool ICollection<T>.IsReadOnly => true;

#pragma warning restore CA1033 // Interface methods should be callable by child types

    object? IList.this[int index]
    {
        get => this[index];
        set => throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public bool Contains(T item, IEqualityComparer<T> comparer)
    {
        return _items.Contains(item, comparer);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public T? Find(Predicate<T> match)
    {
        return _items.Find(match);
    }

    public int FindIndex(Predicate<T> match)
    {
        return _items.FindIndex(match);
    }

    public int FindIndex(int startIndex, Predicate<T> match)
    {
        return _items.FindIndex(startIndex, match);
    }

    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        return _items.FindIndex(startIndex, count, match);
    }

    public int FindLastIndex(Predicate<T> match)
    {
        return _items.FindLastIndex(match);
    }

    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
        return _items.FindLastIndex(startIndex, match);
    }

    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        return _items.FindLastIndex(startIndex, count, match);
    }

    public ReadOnlyCollection<T> FindAll(Predicate<T> match)
    {
        return new(_items.FindAll(match));
    }

    public void ForEach(Action<T> action)
    {
        _items.ForEach(action);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

    public int IndexOf(T item, int index)
    {
        return _items.IndexOf(item, index);
    }

    public int LastIndexOf(T item)
    {
        return _items.LastIndexOf(item);
    }

    public int LastIndexOf(T item, int index)
    {
        return _items.LastIndexOf(item, index);
    }

    public int LastIndexOf(T item, int index, int count)
    {
        return _items.LastIndexOf(item, index, count);
    }

    public ReadOnlyCollection<T> Slice(int start, int length)
    {
        return new(_items.GetRange(start, length));
    }

    public T[] ToArray()
    {
        return [.. _items];
    }

    public override string ToString()
    {
        return CollectionWriter.WriteToString(this);
    }

    int IList.Add(object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.Clear()
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    bool IList.Contains(object? value)
    {
        if (value is not T item)
        {
            return false;
        }

        return Contains(item);
    }

    int IList.IndexOf(object? value)
    {
        if (value is not T item)
        {
            return -1;
        }

        return IndexOf(item);
    }

    void IList.Insert(int index, object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.Remove(object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.RemoveAt(int index)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((IList)_items).CopyTo(array, index);
    }

    void ICollection<T>.Add(T item)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void ICollection<T>.Clear()
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    bool ICollection<T>.Remove(T item)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }
}
