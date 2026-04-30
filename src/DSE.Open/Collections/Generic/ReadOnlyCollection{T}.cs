// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A read-only list backed by a <see cref="List{T}"/>, exposing search, slicing and span access.
/// </summary>
/// <typeparam name="T">The type of element stored in the collection.</typeparam>
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
    /// <summary>
    /// An empty <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    public static readonly ReadOnlyCollection<T> Empty = new([]);

    internal readonly List<T> _items;

    /// <summary>
    /// Initializes a new, empty <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    public ReadOnlyCollection()
    {
        _items = [];
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyCollection{T}"/> containing the elements copied from the specified sequence.
    /// </summary>
    public ReadOnlyCollection(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = [.. items];
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

    /// <inheritdoc/>
    public T this[int index] => _items[index];

    /// <summary>
    /// Searches the entire sorted collection for the specified item using the default comparer.
    /// </summary>
    /// <returns>The zero-based index of the item if found; otherwise, a negative number that is the bitwise complement of the index of the next larger item.</returns>
    public int BinarySearch(T item)
    {
        return BinarySearch(0, Count, item, null);
    }

    /// <summary>
    /// Searches the entire sorted collection for the specified item using the specified comparer.
    /// </summary>
    public int BinarySearch(T item, IComparer<T>? comparer)
    {
        return BinarySearch(0, Count, item, comparer);
    }

    /// <summary>
    /// Searches the specified range of the sorted collection for the specified item.
    /// </summary>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
        return _items.BinarySearch(index, count, item, comparer);
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> over the underlying storage of the collection.
    /// </summary>
    public ReadOnlySpan<T> AsSpan()
    {
        return CollectionsMarshal.AsSpan(_items);
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    /// <summary>
    /// Determines whether the collection contains an element equal to <paramref name="item"/> using the specified comparer.
    /// </summary>
    public bool Contains(T item, IEqualityComparer<T> comparer)
    {
        return _items.Contains(item, comparer);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns the first element that matches the specified predicate, or the default value of <typeparamref name="T"/> if no match is found.
    /// </summary>
    public T? Find(Predicate<T> match)
    {
        return _items.Find(match);
    }

    /// <summary>
    /// Returns the zero-based index of the first element that matches the specified predicate, or -1 if no match is found.
    /// </summary>
    public int FindIndex(Predicate<T> match)
    {
        return _items.FindIndex(match);
    }

    /// <summary>
    /// Returns the zero-based index of the first matching element starting at <paramref name="startIndex"/>, or -1 if no match is found.
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T> match)
    {
        return _items.FindIndex(startIndex, match);
    }

    /// <summary>
    /// Returns the zero-based index of the first matching element within the specified range, or -1 if no match is found.
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        return _items.FindIndex(startIndex, count, match);
    }

    /// <summary>
    /// Returns the zero-based index of the last element that matches the specified predicate, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(Predicate<T> match)
    {
        return _items.FindLastIndex(match);
    }

    /// <summary>
    /// Returns the zero-based index of the last matching element at or before <paramref name="startIndex"/>, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
        return _items.FindLastIndex(startIndex, match);
    }

    /// <summary>
    /// Returns the zero-based index of the last matching element within the specified range, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        return _items.FindLastIndex(startIndex, count, match);
    }

    /// <summary>
    /// Returns a new <see cref="ReadOnlyCollection{T}"/> containing all elements that match the specified predicate.
    /// </summary>
    public ReadOnlyCollection<T> FindAll(Predicate<T> match)
    {
        return new(_items.FindAll(match));
    }

    /// <summary>
    /// Performs the specified action on each element of the collection.
    /// </summary>
    public void ForEach(Action<T> action)
    {
        _items.ForEach(action);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="item"/>, or -1 if not found.
    /// </summary>
    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="item"/> starting at <paramref name="index"/>, or -1 if not found.
    /// </summary>
    public int IndexOf(T item, int index)
    {
        return _items.IndexOf(item, index);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="item"/>, or -1 if not found.
    /// </summary>
    public int LastIndexOf(T item)
    {
        return _items.LastIndexOf(item);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="item"/> at or before <paramref name="index"/>, or -1 if not found.
    /// </summary>
    public int LastIndexOf(T item, int index)
    {
        return _items.LastIndexOf(item, index);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="item"/> within the specified range, or -1 if not found.
    /// </summary>
    public int LastIndexOf(T item, int index, int count)
    {
        return _items.LastIndexOf(item, index, count);
    }

    /// <summary>
    /// Returns a new <see cref="ReadOnlyCollection{T}"/> containing the specified range of elements.
    /// </summary>
    public ReadOnlyCollection<T> Slice(int start, int length)
    {
        return new(_items.GetRange(start, length));
    }

    /// <summary>
    /// Returns a new array containing all elements of the collection.
    /// </summary>
    public T[] ToArray()
    {
        return [.. _items];
    }

    /// <summary>
    /// Returns a string representation of the collection produced by <see cref="CollectionWriter"/>.
    /// </summary>
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
