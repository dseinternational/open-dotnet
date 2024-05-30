// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Collections.Generic;

public class Collection<T> : IList<T>, IReadOnlyList<T>
{
    public static readonly Collection<T> Empty = [];

    private readonly List<T> _items;

    public Collection()
    {
        _items = [];
    }

    public Collection(int count)
    {
        _items = new(count);
    }

    public Collection(IEnumerable<T> collection)
    {
        _items = new(collection);
    }

#pragma warning disable CA1002 // Do not expose generic lists
    protected Collection(List<T> collection)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        Guard.IsNotNull(collection);
        _items = collection;
    }

    public T this[int index]
    {
        get => _items[index];
        set
        {
            EnsureNotReadOnly();
            _items[index] = value;
        }
    }

    public int Count => _items.Count;

    public bool IsReadOnly => ((ICollection<T>)_items).IsReadOnly;

    public void Add(T item)
    {
        EnsureNotReadOnly();
        _items.Add(item);
    }

    public void AddRange(IEnumerable<T> collection)
    {
        EnsureNotReadOnly();
        _items.AddRange(collection);
    }

    public ReadOnlyCollection<T> AsReadOnly()
    {
        return new(this);
    }

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

    public void Clear()
    {
        EnsureNotReadOnly();
        _items.Clear();
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

    public int EnsureCapacity(int capacity)
    {
        EnsureNotReadOnly();
        return _items.EnsureCapacity(capacity);
    }

    private void EnsureNotReadOnly()
    {
        if (IsReadOnly)
        {
            ThrowHelper.ThrowNotSupportedException("Cannot set values in a read only collection.");
        }
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

    public Collection<T> FindAll(Predicate<T> match)
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

    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

    public int IndexOf(T item, int index)
    {
        return _items.IndexOf(item, index);
    }

    public void Insert(int index, T item)
    {
        EnsureNotReadOnly();
        _items.Insert(index, item);
    }

    public void InsertRange(int index, IEnumerable<T> collection)
    {
        EnsureNotReadOnly();
        _items.InsertRange(index, collection);
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

    public bool Remove(T item)
    {
        EnsureNotReadOnly();
        return _items.Remove(item);
    }

    public int RemoveAll(Predicate<T> match)
    {
        EnsureNotReadOnly();
        return _items.RemoveAll(match);
    }

    public void RemoveAt(int index)
    {
        EnsureNotReadOnly();
        _items.RemoveAt(index);
    }

    public void RemoveRange(int index, int count)
    {
        EnsureNotReadOnly();
        _items.RemoveAt(index);
    }

    public void Reverse()
    {
        EnsureNotReadOnly();
        Reverse(0, Count);
    }

    public void Reverse(int index, int count)
    {
        EnsureNotReadOnly();
        _items.Reverse(index, count);
    }

    public Collection<T> Slice(int start, int length)
    {
        return new(_items.GetRange(start, length));
    }

    public void Sort()
    {
        Sort(0, Count, null);
    }

    public void Sort(IComparer<T>? comparer)
    {
        Sort(0, Count, comparer);
    }

    public void Sort(int index, int count, IComparer<T>? comparer)
    {
        EnsureNotReadOnly();
        _items.Sort(index, count, comparer);
    }

    public void Sort(Comparison<T> comparison)
    {
        EnsureNotReadOnly();
        _items.Sort(comparison);
    }

    public T[] ToArray()
    {
        return _items.ToArray();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}
