// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
    "CA1716:Identifiers should not match keywords", Justification = "Preferred name.")]
public abstract class Set
{
    public static Set<T> Create<T>(ISet<T> set)
        where T : IEquatable<T>
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new Set<T>(set);
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    public static Set<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        var set = new HashSet<T>(span.Length);

        for (var i = 0; i < span.Length; i++)
        {
            _ = set.Add(span[i]);
        }
#pragma warning disable IDE0028 // Simplify collection initialization
        return new Set<T>(set);
#pragma warning restore IDE0028 // Simplify collection initialization
    }
}

[CollectionBuilder(typeof(Set), nameof(Create))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
    "CA1716:Identifiers should not match keywords", Justification = "Preferred name.")]
public class Set<T> : Set, ISet<T>
{
    private ISet<T>? _inner;

    public Set()
    {
    }

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

    public bool IsEmpty => _inner is null || Inner.Count == 0;

    public int Count => _inner is null ? 0 : Inner.Count;

    public bool IsReadOnly => true;

    public ReadOnlySet<T> AsReadOnly()
    {
        return new ReadOnlySet<T>(this);
    }

    public bool Contains(T item)
    {
        return _inner is not null && Inner.Contains(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _inner?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return (_inner is null && other.Any()) || Inner.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _inner is not null && Inner.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _inner is null || Inner.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return (_inner is null && !other.Any()) || Inner.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        return _inner is not null && Inner.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        return Inner.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Inner).GetEnumerator();
    }

    public bool Add(T item)
    {
        return Inner.Add(item);
    }

    public void ExceptWith(IEnumerable<T> other)
    {
        Inner.ExceptWith(other);
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        Inner.IntersectWith(other);
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        Inner.SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<T> other)
    {
        Inner.UnionWith(other);
    }

    void ICollection<T>.Add(T item)
    {
        _ = Inner.Add(item);
    }

    public void Clear()
    {
        if (_inner is null)
        {
            return;
        }

        Inner.Clear();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Inner.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return Inner.Remove(item);
    }
}
