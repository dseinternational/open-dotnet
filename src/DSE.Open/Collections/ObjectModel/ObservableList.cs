// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using DSE.Open.Collections.Generic;

// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// A collection that implements <see cref="INotifyCollectionChanged"/> and <see cref="INotifyPropertyChanged"/>
/// and offers more functionality than <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> -
/// for example, sorting in place.
/// </summary>
/// <typeparam name="T">The type of element stored in the collection.</typeparam>
[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
public class ObservableList<T> : IObservableList<T>, IReadOnlyObservableList<T>, IReadOnlyCollection<T>, IList
#pragma warning restore CA1710 // Identifiers should have correct suffix
{
    private static readonly PropertyChangedEventArgs s_countPropertyChangedEventArgs = new(nameof(Count));
    private static readonly PropertyChangedEventArgs s_indexerPropertyChangedEventArgs = new("Item[]");
    private static readonly NotifyCollectionChangedEventArgs s_resetCollectionChangedEventArgs = new(NotifyCollectionChangedAction.Reset);
    private static readonly PropertyChangedEventArgs s_defaultComparisonPropertyChangedEventArgs = new(nameof(DefaultSortComparison));

    private readonly List<T> _observedItems;
    private Comparison<T>? _defaultSortComparison;
    private SimpleMonitor? _monitor; // Lazily allocated only when a subclass calls BlockReentrancy()
    private int _blockReentrancyCount;

    /// <summary>
    /// Initializes a new, empty <see cref="ObservableList{T}"/>.
    /// </summary>
    public ObservableList()
    {
        _observedItems = [];
    }

    /// <summary>
    /// Initializes a new, empty <see cref="ObservableList{T}"/> with the specified initial capacity.
    /// </summary>
    public ObservableList(int capacity)
    {
        Guard.IsGreaterThanOrEqualTo(capacity, 0);

#pragma warning disable IDE0028 // Simplify collection initialization
        _observedItems = new(capacity);
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    /// <summary>
    /// Initializes a new <see cref="ObservableList{T}"/> containing the elements of <paramref name="items"/>.
    /// </summary>
    public ObservableList(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _observedItems = [.. items];
    }

    /// <summary>
    /// Gets the capacity of the underlying list.
    /// </summary>
    public int Capacity => _observedItems.Capacity;

    /// <inheritdoc/>
    public int Count => _observedItems.Count;

    /// <summary>
    /// Gets or sets the default comparison used by parameterless <see cref="Sort()"/> calls.
    /// Setting raises <see cref="PropertyChanged"/>.
    /// </summary>
    public Comparison<T>? DefaultSortComparison
    {
        get => _defaultSortComparison;
        set
        {
            if (value != _defaultSortComparison)
            {
                _defaultSortComparison = value;
                OnPropertyChanged(s_defaultComparisonPropertyChangedEventArgs);
            }
        }
    }

    /// <inheritdoc/>
    public bool IsFixedSize => false;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public bool IsSynchronized => false;

    /// <summary>
    /// Gets an object that can be used to synchronize access to the collection.
    /// </summary>
    public Lock SyncRoot { get; } = new();

#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
    object ICollection.SyncRoot => SyncRoot;
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

    object? IList.this[int index]
    {
        get => _observedItems[index];
        set => SetItem(index, (T)value!);
    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get => _observedItems[index];
        set => SetItem(index, value);
    }

    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public void Add(T item)
    {
        Add(item, false);
    }

    /// <summary>
    /// Adds an item, optionally suppressing notifications.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="suppressEvents">When <see langword="true"/>, no <see cref="CollectionChanged"/> or <see cref="PropertyChanged"/> events are raised.</param>
    public void Add(T item, bool suppressEvents)
    {
        ArgumentNullException.ThrowIfNull(item);

        _observedItems.Add(item);

        OnAdd(item);

        if (!suppressEvents)
        {
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, Count - 1);
        }
    }

    /// <summary>
    /// Adds the elements of <paramref name="collection"/> to the end of the list using the default
    /// <see cref="AddRangeNotification.Add"/> notification.
    /// </summary>
    public void AddRange(IEnumerable<T> collection)
    {
        AddRange(collection, AddRangeNotification.Add);
    }

    /// <summary>
    /// Adds the elements of <paramref name="collection"/> to the end of the list with the specified notification mode.
    /// </summary>
    public void AddRange(IEnumerable<T> collection, AddRangeNotification notification)
    {
        AddRange(collection, notification, false);
    }

    private void AddRange(IEnumerable<T> collection, AddRangeNotification notification, bool suppressEvents)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var collectionAsList = collection as List<T> ?? [.. collection];

        if (collectionAsList.Count == 0)
        {
            return;
        }

        CheckReentrancy();

        if (notification == AddRangeNotification.Reset)
        {
            foreach (var i in collectionAsList)
            {
                _observedItems.Add(i);
            }

            if (!suppressEvents)
            {
                OnPropertyChanged(s_countPropertyChangedEventArgs);
                OnPropertyChanged(s_indexerPropertyChangedEventArgs);
                OnCollectionChanged(s_resetCollectionChangedEventArgs);
            }

            return;
        }

        var startIndex = Count;

        foreach (var i in collectionAsList)
        {
            _observedItems.Add(i);
        }

        if (!suppressEvents)
        {
            OnPropertyChanged(s_countPropertyChangedEventArgs);
            OnPropertyChanged(s_indexerPropertyChangedEventArgs);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, collectionAsList, startIndex);
        }
    }

    /// <summary>
    /// Searches the specified range of the sorted list for <paramref name="item"/> using the specified comparer.
    /// </summary>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observedItems.BinarySearch(index, count, item, comparer);
    }

    /// <summary>
    /// Searches the entire sorted list for <paramref name="item"/> using the default comparer.
    /// </summary>
    public int BinarySearch(T item)
    {
        return BinarySearch(0, Count, item, null);
    }

    /// <summary>
    /// Searches the entire sorted list for <paramref name="item"/> using the specified comparer.
    /// </summary>
    public int BinarySearch(T item, IComparer<T>? comparer)
    {
        return BinarySearch(0, Count, item, comparer);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        CheckReentrancy();

        _observedItems.Clear();

        OnClear();

        OnCountPropertyChanged();
        OnIndexerPropertyChanged();
        OnCollectionReset();
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observedItems.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        _observedItems.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Copies a range of elements from the list to <paramref name="array"/>, starting at <paramref name="arrayIndex"/>.
    /// </summary>
    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
        ArgumentNullException.ThrowIfNull(array);
        _observedItems.CopyTo(index, array, arrayIndex, count);
    }

    /// <summary>
    /// Ensures that the capacity of the underlying list is at least <paramref name="capacity"/>.
    /// </summary>
    public int EnsureCapacity(int capacity)
    {
        return _observedItems.EnsureCapacity(capacity);
    }

    /// <summary>
    /// Returns the first element that matches the specified predicate, or the default value of <typeparamref name="T"/> if no match is found.
    /// </summary>
    public T? Find(Predicate<T> match)
    {
        return _observedItems.Find(match);
    }

    /// <summary>
    /// Returns a new <see cref="ObservableList{T}"/> containing all elements that match the specified predicate.
    /// </summary>
    public ObservableList<T> FindAll(Predicate<T> match)
    {
        return _observedItems.FindAll(match).ToObservableList();
    }

    /// <summary>
    /// Returns the zero-based index of the first element that matches the specified predicate, or -1 if no match is found.
    /// </summary>
    public int FindIndex(Predicate<T> match)
    {
        return _observedItems.FindIndex(match);
    }

    /// <summary>
    /// Returns the zero-based index of the first matching element starting at <paramref name="startIndex"/>, or -1 if no match is found.
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T> match)
    {
        return _observedItems.FindIndex(startIndex, match);
    }

    /// <summary>
    /// Returns the zero-based index of the first matching element within the specified range, or -1 if no match is found.
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        return _observedItems.FindIndex(startIndex, count, match);
    }

    /// <summary>
    /// Returns the last element that matches the specified predicate, or the default value of <typeparamref name="T"/> if no match is found.
    /// </summary>
    public T? FindLast(Predicate<T> match)
    {
        return _observedItems.FindLast(match);
    }

    /// <summary>
    /// Returns the zero-based index of the last element that matches the specified predicate, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(Predicate<T> match)
    {
        return _observedItems.FindLastIndex(match);
    }

    /// <summary>
    /// Returns the zero-based index of the last matching element at or before <paramref name="startIndex"/>, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
        return _observedItems.FindLastIndex(startIndex, match);
    }

    /// <summary>
    /// Returns the zero-based index of the last matching element within the specified range, or -1 if no match is found.
    /// </summary>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        return _observedItems.FindLastIndex(startIndex, count, match);
    }

    /// <summary>
    /// Performs the specified action on each element of the list.
    /// </summary>
    public void ForEach(Action<T> action)
    {
        _observedItems.ForEach(action);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _observedItems.GetEnumerator();
    }

    /// <summary>
    /// Returns a new <see cref="ObservableList{T}"/> containing the specified range of elements.
    /// </summary>
    public ObservableList<T> GetRange(int index, int count)
    {
        return _observedItems.GetRange(index, count).ToObservableList();
    }

    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observedItems.IndexOf(item);
    }

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        Insert(index, item, false);
    }

    private void Insert(int index, T item, bool suppressEvents)
    {
        CheckReentrancy();

        ArgumentNullException.ThrowIfNull(item);

        _observedItems.Insert(index, item);

        OnInsert(index, item);

        if (!suppressEvents)
        {
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }
    }

    /// <summary>
    /// Moves the item at <paramref name="oldIndex"/> to <paramref name="newIndex"/>.
    /// </summary>
    public void MoveItem(int oldIndex, int newIndex)
    {
        MoveItem(oldIndex, newIndex, false);
    }

    private void MoveItem(int oldIndex, int newIndex, bool suppressEvents)
    {
        CheckReentrancy();

        var item = this[oldIndex];

        _observedItems.RemoveAt(oldIndex);
        _observedItems.Insert(newIndex, item);

        OnMoveItem(newIndex, newIndex);

        if (!suppressEvents)
        {
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
        }
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        return Remove(item, false);
    }

    private bool Remove(T item, bool suppressEvents)
    {
        var index = IndexOf(item);

        if (index > -1)
        {
            RemoveAt(index, suppressEvents);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        RemoveAt(index, false);
    }

    private void RemoveAt(int index, bool suppressEvents)
    {
        CheckReentrancy();

        var removedItem = this[index];

        _observedItems.RemoveAt(index);

        OnRemoveAt(index, removedItem);

        if (!suppressEvents)
        {
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }
    }

    /// <summary>
    /// Removes each element of <paramref name="collection"/> from the list using the default
    /// <see cref="RemoveRangeNotification.Reset"/> notification.
    /// </summary>
    public void RemoveRange(IEnumerable<T> collection)
    {
        RemoveRange(collection, RemoveRangeNotification.Reset);
    }

    /// <summary>
    /// Removes each element of <paramref name="collection"/> from the list with the specified notification mode.
    /// </summary>
    public void RemoveRange(IEnumerable<T> collection, RemoveRangeNotification notification)
    {
        RemoveRange(collection, notification, false);
    }

    private void RemoveRange(IEnumerable<T> collection, RemoveRangeNotification notification, bool suppressEvents)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var collectionAsList = collection as List<T> ?? [.. collection];

        if (collectionAsList.Count == 0)
        {
            return;
        }

        CheckReentrancy();

        if (notification == RemoveRangeNotification.Reset)
        {
            foreach (var i in collectionAsList)
            {
                _ = Remove(i, true);
            }

            if (!suppressEvents)
            {
                OnCollectionChanged(s_resetCollectionChangedEventArgs);
            }

            return;
        }

        for (var i = 0; i < collectionAsList.Count; i++)
        {
            if (!Remove(collectionAsList[i], true))
            {
                collectionAsList.RemoveAt(i);
                i--;
            }
        }

        if (collectionAsList.Count == 0)
        {
            return;
        }

        if (!suppressEvents)
        {
            OnPropertyChanged(s_countPropertyChangedEventArgs);
            OnPropertyChanged(s_indexerPropertyChangedEventArgs);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, collectionAsList, -1);
        }
    }

    /// <summary>
    /// Replaces the item at <paramref name="index"/> with <paramref name="item"/> and raises change notifications.
    /// </summary>
    protected void SetItem(int index, T item)
    {
        CheckReentrancy();

        ArgumentNullException.ThrowIfNull(item);

        var oldItem = this[index];

        _observedItems[index] = item;

        OnSetItem(index, oldItem, item);

        OnIndexerPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
    }

    /// <summary>
    /// Sets the items in the instance to match those in the specified collection -
    /// adding and removing existing items as appropriate. Does not apply any sort.
    /// </summary>
    /// <param name="items"></param>
    public void SetRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        CheckReentrancy();

        var targetItems = items as IList<T> ?? [.. items];

        if (targetItems.Count == 0)
        {
            Clear();
            return;
        }

        for (var newIndex = 0; newIndex < targetItems.Count; newIndex++)
        {
            var newItem = targetItems[newIndex];
            var oldIndex = IndexOf(targetItems[newIndex]);

            if (oldIndex < 0)
            {
                Insert(newIndex, newItem, true);
            }
            else if (oldIndex != newIndex)
            {
                MoveItem(oldIndex, newIndex, true);
            }
        }

        var toRemove = this.Where(i => targetItems.IndexOf(i) < 0).ToArray();

        RemoveRange(toRemove, RemoveRangeNotification.Reset, true);

        OnCollectionChanged(s_resetCollectionChangedEventArgs);
    }

    /// <summary>
    /// Returns a new <see cref="ObservableList{T}"/> containing the specified range of elements.
    /// </summary>
    public ObservableList<T> Slice(int start, int length)
    {
        return GetRange(start, length);
    }

    /// <summary>
    /// Sorts the list using the default comparer in the specified order.
    /// </summary>
    public void Sort(SortOrder sortOrder)
    {
        Sort(sortOrder, (IComparer<T>?)null);
    }

    /// <summary>
    /// Sorts the list in ascending order using the specified comparer.
    /// </summary>
    public void Sort(IComparer<T>? comparer)
    {
        Sort(SortOrder.Ascending, (IComparer<T>?)null);
    }

    /// <summary>
    /// Sorts the collection using the specified comparer and sort order (the comparer should
    /// provide an ascending comparison).
    /// </summary>
    /// <param name="sortOrder">Specifies whether to order ascending or descending.</param>
    /// <param name="comparer">The comparer to use for sorting (provides an ascending comparison).</param>
    public void Sort(SortOrder sortOrder, IComparer<T>? comparer)
    {
        var comparison = comparer is not null
            ? comparer.Compare
            : DefaultSortComparison ?? Comparer<T>.Default.Compare;

        Sort(sortOrder, comparison);
    }

    /// <summary>
    /// Sorts the collection using the specified comparison in ascending order.
    /// </summary>
    public void Sort()
    {
        Sort(SortOrder.Ascending, DefaultSortComparison ?? Comparer<T>.Default.Compare);
    }

    /// <summary>
    /// Sorts the list in ascending order using the specified comparison.
    /// </summary>
    public void Sort(Comparison<T> comparison)
    {
        Sort(SortOrder.Ascending, comparison);
    }

    /// <summary>
    /// Sorts the collection using the specified comparison and sort order (the comparison should
    /// provide an ascending comparison).
    /// </summary>
    /// <param name="sortOrder">Specifies whether to order ascending or descending.</param>
    /// <param name="comparison">The comparison to use for sorting (provides an ascending comparison).</param>
    public void Sort(SortOrder sortOrder, Comparison<T> comparison)
    {
        CheckReentrancy();

        ArgumentNullException.ThrowIfNull(comparison);

        var sorted = new List<T>(_observedItems);

        var orderedComparison = sortOrder == SortOrder.Ascending ? comparison : (a, b) => comparison(a, b) * -1;

        sorted.Sort(orderedComparison);

        for (var sortedIndex = 0; sortedIndex < sorted.Count; sortedIndex++)
        {
            var currentIndex = _observedItems.IndexOf(sorted[sortedIndex]);

            if (sortedIndex != currentIndex)
            {
                MoveItem(currentIndex, sortedIndex);
            }
        }

        OnSort(sortOrder, comparison);
    }

    /// <summary>
    /// Sorts the list in ascending order by the key returned by <paramref name="keySelector"/>.
    /// </summary>
    public void SortBy<TKey>(Func<T, TKey> keySelector)
    {
        SortBy(keySelector, (IComparer<TKey>?)null);
    }

    /// <summary>
    /// Sorts the list by the key returned by <paramref name="keySelector"/> in the specified order.
    /// </summary>
    public void SortBy<TKey>(SortOrder sortOrder, Func<T, TKey> keySelector)
    {
        SortBy(sortOrder, keySelector, (IComparer<TKey>?)null);
    }

    /// <summary>
    /// Sorts the list in ascending order by the key returned by <paramref name="keySelector"/> using the specified comparer.
    /// </summary>
    public void SortBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer)
    {
        SortBy(SortOrder.Ascending, keySelector, comparer);
    }

    /// <summary>
    /// Sorts the list by the key returned by <paramref name="keySelector"/> in the specified order using the specified comparer.
    /// </summary>
    public void SortBy<TKey>(SortOrder sortOrder, Func<T, TKey> keySelector, IComparer<TKey>? comparer)
    {
        ArgumentNullException.ThrowIfNull(keySelector);

        comparer ??= Comparer<TKey>.Default;

        SortBy(sortOrder, keySelector, comparer.Compare);
    }

    /// <summary>
    /// Sorts the list in ascending order by the key returned by <paramref name="keySelector"/> using the specified comparison.
    /// </summary>
    public void SortBy<TKey>(Func<T, TKey> keySelector, Comparison<TKey> comparison)
    {
        SortBy(SortOrder.Ascending, keySelector, comparison);
    }

    /// <summary>
    /// Sorts the list by the key returned by <paramref name="keySelector"/> in the specified order using the specified comparison.
    /// </summary>
    public void SortBy<TKey>(SortOrder sortOrder, Func<T, TKey> keySelector, Comparison<TKey> comparison)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(comparison);

        Sort(sortOrder, itemComparison);

        int itemComparison(T a, T b)
        {
            return comparison(keySelector(a), keySelector(b));
        }
    }

    /// <summary>
    /// Sets the capacity to the actual number of elements in the list.
    /// </summary>
    public void TrimExcess()
    {
        _observedItems.TrimExcess();
    }

    /// <summary>
    /// Determines whether every element of the list satisfies the specified predicate.
    /// </summary>
    public bool TrueForAll(Predicate<T> match)
    {
        return _observedItems.TrueForAll(match);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _observedItems.GetEnumerator();
    }

    int IList.Add(object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Guard.IsOfType<T>(value);

        Add((T)value);
        return Count - 1;
    }

    bool IList.Contains(object? value)
    {
        return IsCompatibleObject(value) && Contains((T)value!);
    }

    int IList.IndexOf(object? value)
    {
        return IsCompatibleObject(value) ? IndexOf((T)value!) : -1;
    }

    void IList.Insert(int index, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Guard.IsOfType<T>(value);

        Insert(index, (T)value);
    }

    void IList.Remove(object? value)
    {
        if (IsCompatibleObject(value))
        {
            _ = Remove((T)value!);
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((ICollection)_observedItems).CopyTo(array, index);
    }

    /// <summary>
    /// Disallow reentrant attempts to change this collection. E.g. an event handler
    /// of the CollectionChanged event is not allowed to make changes to this collection.
    /// </summary>
    /// <remarks>
    /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
    /// <code>
    ///         using (BlockReentrancy())
    ///         {
    ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
    ///         }
    /// </code>
    /// </remarks>
    protected IDisposable BlockReentrancy()
    {
        _blockReentrancyCount++;
        return EnsureMonitorInitialized();
    }

    /// <summary>
    /// Throws if a reentrant change is attempted while a <see cref="CollectionChanged"/> event is being raised.
    /// </summary>
    protected void CheckReentrancy()
    {
        if (_blockReentrancyCount > 0)
        {
            // we can allow changes if there's only one listener - the problem
            // only arises if reentrant changes make the original event args
            // invalid for later listeners.  This keeps existing code working
            // (e.g. Selector.SelectedItems).
            if (CollectionChanged?.GetInvocationList().Length > 1)
            {
                throw new InvalidOperationException(
                    $"Cannot change {nameof(ObservableList<>)} during a {nameof(CollectionChanged)} event.");
            }
        }
    }

    /// <summary>
    /// Called after an item has been added to the list.
    /// </summary>
    protected virtual void OnAdd(T item)
    {
    }

    /// <summary>
    /// Called after an item has been inserted into the list.
    /// </summary>
    protected virtual void OnInsert(int index, T item)
    {
    }

    /// <summary>
    /// Called after an item has been moved within the list.
    /// </summary>
    protected virtual void OnMoveItem(int oldIndex, int newIndex)
    {
    }

    /// <summary>
    /// Called after an item has been replaced via the indexer.
    /// </summary>
    protected virtual void OnSetItem(int index, T oldItem, T item)
    {
    }

    /// <summary>
    /// Called after an item has been removed from the list.
    /// </summary>
    protected virtual void OnRemoveAt(int index, T removedItem)
    {
    }

    /// <summary>
    /// Called after the list has been cleared.
    /// </summary>
    protected virtual void OnClear()
    {
    }

    /// <summary>
    /// Called after the list has been sorted.
    /// </summary>
    protected virtual void OnSort(SortOrder sortOrder, Comparison<T> comparison)
    {
    }

    /// <summary>
    /// Raises the <see cref="CollectionChanged"/> event with the specified arguments.
    /// </summary>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        var handler = CollectionChanged;

        if (handler != null)
        {
            // Not calling BlockReentrancy() here to avoid the SimpleMonitor allocation
            _blockReentrancyCount++;
            try
            {
                handler(this, e);
            }
            finally
            {
                _blockReentrancyCount--;
            }
        }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event with the specified arguments.
    /// </summary>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    private void OnCountPropertyChanged()
    {
        OnPropertyChanged(s_countPropertyChangedEventArgs);
    }

    private void OnIndexerPropertyChanged()
    {
        OnPropertyChanged(s_indexerPropertyChangedEventArgs);
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index)
    {
        OnCollectionChanged(new(action, item, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index, int oldIndex)
    {
        OnCollectionChanged(new(action, item, index, oldIndex));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? oldItem, object? newItem, int index)
    {
        OnCollectionChanged(new(action, newItem, oldItem, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, IList? changedItems, int startIndex)
    {
        OnCollectionChanged(new(action, changedItems, startIndex));
    }

    private void OnCollectionReset()
    {
        OnCollectionChanged(s_resetCollectionChangedEventArgs);
    }

    private static bool IsCompatibleObject(object? value)
    {
        return value is T || (value == null && default(T) == null);
    }

    private SimpleMonitor EnsureMonitorInitialized()
    {
        return _monitor ??= new(this);
    }

    private sealed class SimpleMonitor : IDisposable
    {
        internal ObservableList<T> _collection;

        public SimpleMonitor(ObservableList<T> collection)
        {
            Debug.Assert(collection is not null);
            _collection = collection;
        }

        public void Dispose()
        {
            _collection._blockReentrancyCount--;
        }
    }
}
