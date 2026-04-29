// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Provides factory methods for <see cref="ReadOnlyObservableSet{T}"/>.
/// </summary>
public static class ReadOnlyObservableSet
{
    /// <summary>
    /// Returns a cached empty <see cref="ReadOnlyObservableSet{T}"/>.
    /// </summary>
    public static ReadOnlyObservableSet<T> Empty<T>()
    {
        return EmptyReadOnlyObservableSet<T>.s_value;
    }

    private static class EmptyReadOnlyObservableSet<T>
    {
        internal static readonly ReadOnlyObservableSet<T> s_value = new();
    }
}

/// <summary>
/// A read-only view over an <see cref="IObservableSet{T}"/>.
/// </summary>
/// <typeparam name="T">The type of element stored in the set.</typeparam>
public class ReadOnlyObservableSet<T> : IReadOnlyObservableSet<T>
{
    private readonly IObservableSet<T> _inner;

    internal ReadOnlyObservableSet() : this([])
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyObservableSet{T}"/> wrapping a new <see cref="ObservableSet{T}"/> populated from <paramref name="collection"/>.
    /// </summary>
    public ReadOnlyObservableSet(IEnumerable<T> collection) : this(new ObservableSet<T>(collection))
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyObservableSet{T}"/> wrapping the specified observable set.
    /// </summary>
    public ReadOnlyObservableSet(IObservableSet<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        _inner = inner;
    }

    /// <inheritdoc/>
    public int Count => _inner.Count;

    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add => _inner.CollectionChanged += value;
        remove => _inner.CollectionChanged -= value;
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _inner.PropertyChanged += value;
        remove => _inner.PropertyChanged -= value;
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _inner.Contains(item);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return _inner.IsProperSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _inner.IsProperSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _inner.IsSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _inner.IsSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
    {
        return _inner.Overlaps(other);
    }

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
    {
        return _inner.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_inner).GetEnumerator();
    }
}
