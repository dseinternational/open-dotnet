// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Provides factory methods for creating <see cref="Set{T}"/> instances.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
    "CA1716:Identifiers should not match keywords", Justification = "Preferred name.")]
public static class Set
{
    /// <summary>
    /// Creates a <see cref="Set{T}"/> wrapping the specified set.
    /// </summary>
    public static Set<T> Create<T>(ISet<T> set)
        where T : IEquatable<T>
    {
        return new Set<T>(set);
    }

    /// <summary>
    /// Creates a <see cref="Set{T}"/> containing the distinct elements of the specified span.
    /// </summary>
    public static Set<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        var set = new HashSet<T>(span.Length);

        for (var i = 0; i < span.Length; i++)
        {
            _ = set.Add(span[i]);
        }

        return new Set<T>(set);
    }
}

/// <summary>
/// A lazily-initialized set that defers creation of the underlying <see cref="HashSet{T}"/>
/// until it is needed.
/// </summary>
/// <typeparam name="T">The type of element stored in the set.</typeparam>
[CollectionBuilder(typeof(Set), nameof(Set.Create))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
    "CA1716:Identifiers should not match keywords", Justification = "Preferred name.")]
public class Set<T> : ISet<T>
{
    private ISet<T>? _inner;

    /// <summary>
    /// Initializes a new, empty <see cref="Set{T}"/>. The underlying storage is allocated lazily.
    /// </summary>
    public Set()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Set{T}"/> containing the distinct elements of <paramref name="set"/>.
    /// </summary>
    public Set(IEnumerable<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set is IReadOnlyCollection<T> col)
        {
            if (col.Count > 0)
            {
                _inner = new HashSet<T>(col);
            }
        }
        else if (set.GetEnumerator().MoveNext())
        {
            _inner = new HashSet<T>(set);
        }
    }

    /// <summary>
    /// Initializes a new <see cref="Set{T}"/> wrapping the specified set as its backing store.
    /// </summary>
    public Set(ISet<T> set)
    {
        _inner = set;
    }

    private ISet<T> Inner
    {
        get
        {
            _inner ??= new HashSet<T>();
            return _inner;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the set is empty.
    /// </summary>
    public bool IsEmpty => _inner is null || Inner.Count == 0;

    /// <inheritdoc/>
    public int Count => _inner is null ? 0 : Inner.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <summary>
    /// Returns a <see cref="ReadOnlySet{T}"/> view over this set.
    /// </summary>
    public ReadOnlySet<T> AsReadOnly()
    {
        return new ReadOnlySet<T>(this);
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _inner is not null && Inner.Contains(item);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _inner?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
    }

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return (_inner is null && other.Any()) || Inner.IsProperSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _inner is not null && Inner.IsProperSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _inner is null || Inner.IsSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return (_inner is null && !other.Any()) || Inner.IsSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
    {
        return _inner is not null && Inner.Overlaps(other);
    }

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
    {
        return Inner.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Inner).GetEnumerator();
    }

    /// <inheritdoc/>
    public bool Add(T item)
    {
        return Inner.Add(item);
    }

    /// <inheritdoc/>
    public void ExceptWith(IEnumerable<T> other)
    {
        Inner.ExceptWith(other);
    }

    /// <inheritdoc/>
    public void IntersectWith(IEnumerable<T> other)
    {
        Inner.IntersectWith(other);
    }

    /// <inheritdoc/>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        Inner.SymmetricExceptWith(other);
    }

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        Inner.UnionWith(other);
    }

    void ICollection<T>.Add(T item)
    {
        _ = Inner.Add(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (_inner is null)
        {
            return;
        }

        Inner.Clear();
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        Inner.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        return Inner.Remove(item);
    }
}
