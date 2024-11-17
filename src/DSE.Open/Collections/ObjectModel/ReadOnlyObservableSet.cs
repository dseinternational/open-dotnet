// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

public static class ReadOnlyObservableSet
{
    public static ReadOnlyObservableSet<T> Empty<T>()
    {
        return EmptyReadOnlyObservableSet<T>.s_value;
    }

    private static class EmptyReadOnlyObservableSet<T>
    {
        internal static readonly ReadOnlyObservableSet<T> s_value = new();
    }
}

public class ReadOnlyObservableSet<T> : IReadOnlyObservableSet<T>
{
    private readonly IObservableSet<T> _inner;

    internal ReadOnlyObservableSet() : this([])
    {
    }

    public ReadOnlyObservableSet(IEnumerable<T> collection) : this(new ObservableSet<T>(collection))
    {
    }

    public ReadOnlyObservableSet(IObservableSet<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        _inner = inner;
    }

    public int Count => _inner.Count;

    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add => _inner.CollectionChanged += value;
        remove => _inner.CollectionChanged -= value;
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _inner.PropertyChanged += value;
        remove => _inner.PropertyChanged -= value;
    }

    public bool Contains(T item)
    {
        return _inner.Contains(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return _inner.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _inner.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _inner.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _inner.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        return _inner.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        return _inner.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_inner).GetEnumerator();
    }
}
