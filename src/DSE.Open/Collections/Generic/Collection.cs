// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.InteropServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A list-backed collection that exposes mutation, search, sort and slicing helpers and that
/// implements <see cref="IList{T}"/>, <see cref="IReadOnlyList{T}"/> and <see cref="IList"/>.
/// </summary>
/// <typeparam name="T">The type of element stored in the collection.</typeparam>
public class Collection<T> : IList<T>, IReadOnlyList<T>, IList
{
    /// <summary>
    /// An empty <see cref="Collection{T}"/>.
    /// </summary>
    public static readonly Collection<T> Empty = [];

    private readonly List<T> _items;

    /// <summary>
    /// Initializes a new, empty <see cref="Collection{T}"/>.
    /// </summary>
    public Collection()
    {
        _items = [];
    }

    /// <summary>
    /// Initializes a new, empty <see cref="Collection{T}"/> with the specified initial capacity.
    /// </summary>
    /// <param name="count">The initial capacity of the underlying list.</param>
    public Collection(int count)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        _items = new(count);
#pragma warning restore IDE0028 // Simplify collection initialization

    }

    /// <summary>
    /// Initializes a new <see cref="Collection{T}"/> containing the elements copied from the specified sequence.
    /// </summary>
    /// <param name="collection">The sequence whose elements are copied to the new collection.</param>
    public Collection(IEnumerable<T> collection)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0306 // Simplify collection initialization
        _items = new(collection);
#pragma warning restore IDE0306 // Simplify collection initialization
#pragma warning restore IDE0028 // Simplify collection initialization

    }

    /// <summary>
    /// Initializes a new <see cref="Collection{T}"/> that wraps the specified <see cref="List{T}"/> as its backing store.
    /// </summary>
    /// <param name="collection">The list to wrap; subsequent operations mutate this list directly.</param>
#pragma warning disable CA1002 // Do not expose generic lists
    protected Collection(List<T> collection)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        ArgumentNullException.ThrowIfNull(collection);
        _items = collection;
    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get => _items[index];
        set
        {
            EnsureNotReadOnly();
            _items[index] = value;
        }
    }

    /// <inheritdoc/>
    public int Count => _items.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<T>)_items).IsReadOnly;

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool IList.IsFixedSize => ((IList)_items).IsFixedSize;

    bool ICollection.IsSynchronized => ((ICollection)_items).IsSynchronized;

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <summary>
    /// Gets an object that can be used to synchronize access to the collection.
    /// </summary>
    public Lock SyncRoot { get; } = new();

#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
    object ICollection.SyncRoot => SyncRoot;
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

    object? IList.this[int index] { get => this[index]; set => this[index] = (T)value!; }

    /// <inheritdoc/>
    public void Add(T item)
    {
        EnsureNotReadOnly();
        _items.Add(item);
    }

    /// <summary>
    /// Adds the elements of the specified sequence to the end of the collection.
    /// </summary>
    /// <param name="collection">The sequence whose elements should be added.</param>
    public void AddRange(IEnumerable<T> collection)
    {
        EnsureNotReadOnly();
        _items.AddRange(collection);
    }

    /// <summary>
    /// Returns a snapshot of the current contents as a new <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    public ReadOnlyCollection<T> AsReadOnly()
    {
        return [.. this];
    }

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> over the underlying storage of the collection.
    /// </summary>
    public ReadOnlySpan<T> AsSpan()
    {
        return CollectionsMarshal.AsSpan(_items);
    }

    /// <summary>
    /// Searches the entire sorted collection for the specified item using the default comparer.
    /// </summary>
    /// <param name="item">The item to locate.</param>
    /// <returns>The zero-based index of the item if found; otherwise, a negative number that is the bitwise complement of the index of the next larger item.</returns>
    public int BinarySearch(T item)
    {
        return BinarySearch(0, Count, item, null);
    }

    /// <summary>
    /// Searches the entire sorted collection for the specified item using the specified comparer.
    /// </summary>
    /// <param name="item">The item to locate.</param>
    /// <param name="comparer">The comparer to use, or <see langword="null"/> for the default.</param>
    /// <returns>The zero-based index of the item if found; otherwise, a negative number that is the bitwise complement of the index of the next larger item.</returns>
    public int BinarySearch(T item, IComparer<T>? comparer)
    {
        return BinarySearch(0, Count, item, comparer);
    }

    /// <summary>
    /// Searches a range of the sorted collection for the specified item using the specified comparer.
    /// </summary>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="count">The length of the range to search.</param>
    /// <param name="item">The item to locate.</param>
    /// <param name="comparer">The comparer to use, or <see langword="null"/> for the default.</param>
    /// <returns>The zero-based index of the item if found; otherwise, a negative number that is the bitwise complement of the index of the next larger item.</returns>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
        return _items.BinarySearch(index, count, item, comparer);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        EnsureNotReadOnly();
        _items.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    /// <summary>
    /// Determines whether the collection contains an element equal to <paramref name="item"/> using the specified comparer.
    /// </summary>
    /// <param name="item">The item to locate.</param>
    /// <param name="comparer">The equality comparer to use.</param>
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
    /// Ensures that the capacity of the underlying list is at least <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The minimum required capacity.</param>
    /// <returns>The new capacity of the underlying list.</returns>
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
    /// Returns a new <see cref="Collection{T}"/> containing all elements that match the specified predicate.
    /// </summary>
    public Collection<T> FindAll(Predicate<T> match)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new(_items.FindAll(match));
#pragma warning restore IDE0028 // Simplify collection initialization
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        EnsureNotReadOnly();
        _items.Insert(index, item);
    }

    /// <summary>
    /// Inserts the elements of the specified sequence into the collection at <paramref name="index"/>.
    /// </summary>
    public void InsertRange(int index, IEnumerable<T> collection)
    {
        EnsureNotReadOnly();
        _items.InsertRange(index, collection);
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

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        EnsureNotReadOnly();
        return _items.Remove(item);
    }

    /// <summary>
    /// Removes all elements that match the specified predicate.
    /// </summary>
    /// <returns>The number of elements removed.</returns>
    public int RemoveAll(Predicate<T> match)
    {
        EnsureNotReadOnly();
        return _items.RemoveAll(match);
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        EnsureNotReadOnly();
        _items.RemoveAt(index);
    }

    /// <summary>
    /// Removes a range of elements from the collection.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range.</param>
    /// <param name="count">The number of elements to remove.</param>
    public void RemoveRange(int index, int count)
    {
        EnsureNotReadOnly();
        _items.RemoveAt(index);
    }

    /// <summary>
    /// Reverses the order of the elements in the entire collection.
    /// </summary>
    public void Reverse()
    {
        EnsureNotReadOnly();
        Reverse(0, Count);
    }

    /// <summary>
    /// Reverses the order of the elements in the specified range of the collection.
    /// </summary>
    public void Reverse(int index, int count)
    {
        EnsureNotReadOnly();
        _items.Reverse(index, count);
    }

    /// <summary>
    /// Returns a new <see cref="Collection{T}"/> containing the specified range of elements.
    /// </summary>
    /// <param name="start">The zero-based starting index of the range.</param>
    /// <param name="length">The number of elements in the range.</param>
    public Collection<T> Slice(int start, int length)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new(_items.GetRange(start, length));
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    /// <summary>
    /// Sorts the elements of the entire collection using the default comparer.
    /// </summary>
    public void Sort()
    {
        Sort(0, Count, null);
    }

    /// <summary>
    /// Sorts the elements of the entire collection using the specified comparer.
    /// </summary>
    public void Sort(IComparer<T>? comparer)
    {
        Sort(0, Count, comparer);
    }

    /// <summary>
    /// Sorts the elements within the specified range using the specified comparer.
    /// </summary>
    public void Sort(int index, int count, IComparer<T>? comparer)
    {
        EnsureNotReadOnly();
        _items.Sort(index, count, comparer);
    }

    /// <summary>
    /// Sorts the elements of the entire collection using the specified <see cref="Comparison{T}"/>.
    /// </summary>
    public void Sort(Comparison<T> comparison)
    {
        EnsureNotReadOnly();
        _items.Sort(comparison);
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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    int IList.Add(object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Add((T)value);
        return Count - 1;
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
        ArgumentNullException.ThrowIfNull(value);
        Insert(index, (T)value);
    }

    void IList.Remove(object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _ = Remove((T)value);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        _items.CopyTo((T[])array, index);
    }
}
