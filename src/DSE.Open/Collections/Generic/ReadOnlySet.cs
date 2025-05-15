// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

public abstract class ReadOnlySet
{
    public static ReadOnlySet<T> Create<T>(IReadOnlySet<T> set)
        where T : IEquatable<T>
    {
        return new ReadOnlySet<T>(set);
    }

    public static ReadOnlySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        var set = new HashSet<T>(span.Length);

        for (var i = 0; i < span.Length; i++)
        {
            _ = set.Add(span[i]);
        }
#pragma warning disable IDE0028 // Simplify collection initialization
        return new ReadOnlySet<T>(set);
#pragma warning restore IDE0028 // Simplify collection initialization
    }
}

[CollectionBuilder(typeof(ReadOnlySet), nameof(Create))]
public class ReadOnlySet<T> : Set, IReadOnlySet<T>
{
    public static readonly ReadOnlySet<T> Empty = new(new HashSet<T>());

    private readonly IReadOnlySet<T> _inner;

    public ReadOnlySet()
    {
        _inner = Empty;
    }

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

    public ReadOnlySet(IReadOnlySet<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);
        _inner = set;
    }

    public bool IsEmpty => _inner.Count == 0;

    public int Count => _inner.Count;

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

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
