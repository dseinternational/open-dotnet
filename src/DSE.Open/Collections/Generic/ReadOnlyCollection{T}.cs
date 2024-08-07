﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

[CollectionBuilder(typeof(ReadOnlyCollection), nameof(ReadOnlyCollection.Create))]
public class ReadOnlyCollection<T> : IReadOnlyList<T>
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
    internal static ReadOnlyCollection<T> CreateWrapped(List<T> items)
    {
        return new(items);
    }

#pragma warning disable CA1002 // Do not expose generic lists
    protected ReadOnlyCollection(List<T> collection)
#pragma warning restore CA1002 // Do not expose generic lists
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
        return _items.ToArray();
    }
}
