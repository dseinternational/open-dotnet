// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

public static class ReadOnlyObservableList
{
    public static IReadOnlyObservableList<T> Empty<T>()
    {
        return EmptyReadOnlyObservableList<T>.s_value;
    }

    private static class EmptyReadOnlyObservableList<T>
    {
        internal static readonly ReadOnlyObservableList<T> s_value = new();
    }
}

/// <summary>
/// A read-only view of an observable list.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReadOnlyObservableList<T> : IReadOnlyObservableList<T>
{
    private readonly IObservableList<T> _inner;

    internal ReadOnlyObservableList() : this([])
    {
    }

    public ReadOnlyObservableList(IEnumerable<T> collection)
        : this(new ObservableList<T>(collection))
    {
    }

    public ReadOnlyObservableList(IObservableList<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        _inner = collection;
    }

    public T this[int index] => _inner[index];

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

    public IEnumerator<T> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
