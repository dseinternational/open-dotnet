// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Provides factory methods for creating <see cref="ReadOnlySet{T}"/> instances.
/// </summary>
public static class ReadOnlySet
{
    /// <summary>
    /// Creates a <see cref="ReadOnlySet{T}"/> wrapping the specified set.
    /// </summary>
    public static ReadOnlySet<T> Create<T>(IReadOnlySet<T> set)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set.Count == 0)
        {
            return ReadOnlySet<T>.Empty;
        }

        return new ReadOnlySet<T>(set);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlySet{T}"/> containing the distinct elements of the specified span.
    /// </summary>
    public static ReadOnlySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.Length == 0)
        {
            return ReadOnlySet<T>.Empty;
        }

        var set = new HashSet<T>(span.Length);

        for (var i = 0; i < span.Length; i++)
        {
            _ = set.Add(span[i]);
        }

        return new ReadOnlySet<T>(set);
    }
}

/// <summary>
/// A read-only wrapper around an <see cref="IReadOnlySet{T}"/>.
/// </summary>
/// <typeparam name="T">The type of element stored in the set.</typeparam>
[CollectionBuilder(typeof(ReadOnlySet), nameof(ReadOnlySet.Create))]
public class ReadOnlySet<T> : IReadOnlySet<T>
{
    /// <summary>
    /// An empty <see cref="ReadOnlySet{T}"/>.
    /// </summary>
    public static readonly ReadOnlySet<T> Empty = new(new HashSet<T>());

    private readonly IReadOnlySet<T> _inner;

    /// <summary>
    /// Initializes a new, empty <see cref="ReadOnlySet{T}"/>.
    /// </summary>
    public ReadOnlySet()
    {
        _inner = Empty;
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlySet{T}"/> containing the distinct elements of <paramref name="set"/>.
    /// </summary>
    public ReadOnlySet(IEnumerable<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set is IReadOnlyCollection<T> col)
        {
            if (col.Count > 0)
            {
                _inner = new HashSet<T>(col);
            }
            else
            {
                _inner = Empty;
            }
        }
        else if (set.GetEnumerator().MoveNext())
        {
            _inner = new HashSet<T>(set);
        }
        else
        {
            _inner = Empty;
        }
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlySet{T}"/> wrapping the specified read-only set.
    /// </summary>
    public ReadOnlySet(IReadOnlySet<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);
        _inner = set;
    }

    /// <summary>
    /// Gets a value indicating whether the set is empty.
    /// </summary>
    public bool IsEmpty => _inner.Count == 0;

    /// <inheritdoc/>
    public int Count => _inner.Count;

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

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
