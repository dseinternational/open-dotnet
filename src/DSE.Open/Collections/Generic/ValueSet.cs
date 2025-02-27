// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A set of initialization methods for instances of <see cref="ValueSet{T}"/>.
/// </summary>
public static class ValueSet
{
    /// <summary>
    /// Creates an <see cref="ValueSet{T}"/> with the specified elements.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the set.</typeparam>
    /// <param name="items">The elements to store in the set.</param>
    /// <returns>A <see cref="ValueSet{T}"/> containing the specified items.</returns>
    public static ValueSet<T> Create<T>(ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ValueSet<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        var set = new HashSet<T>(items.Length);

        foreach (var item in items)
        {
            _ = set.Add(item);
        }

        return new(set);
    }
}

/// <summary>
/// A <see cref="ISet{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks><see cref="ICollection{T}"/> is implemented explicitly to support deserialization.</remarks>
[CollectionBuilder(typeof(ValueSet), nameof(ValueSet.Create))]
public class ValueSet<T> : ISet<T>, IEquatable<ValueSet<T>>, ICollection<T>
{
    public static readonly ValueSet<T> Empty = [.. Enumerable.Empty<T>()];

    private readonly HashSet<T> _set;

    public ValueSet()
    {
        _set = [];
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal ValueSet(HashSet<T> set)
    {
        _set = set;
    }

    /// <summary>
    /// Provides a method for derived types to directly set the backing store. <paramref name="noCopy"/> must be <c>true</c>.
    /// </summary>
    /// <param name="set">The set to use as the backing store.</param>
    /// <param name="noCopy">Must be true.</param>
    protected ValueSet(HashSet<T> set, bool noCopy)
    {
        Debug.Assert(noCopy);
        _set = set;
    }

    public ValueSet(IEnumerable<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set is ValueSet<T> other)
        {
            _set = other._set;
        }

        _set = new(set);
    }

    public int Count => _set.Count;

#pragma warning disable CA1033 // Interface methods should be callable by child types
    bool ICollection<T>.IsReadOnly => false;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public bool Add(T item)
    {
        return Add(item);
    }

    void ICollection<T>.Add(T item)
    {
        _ = _set.Add(item);
    }

    public void Clear()
    {
        _set.Clear();
    }

    public bool Remove(T item)
    {
        return _set.Remove(item);
    }

    public bool Contains(T item)
    {
        return _set.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _set.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _set.GetEnumerator();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return _set.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _set.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _set.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _set.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        return _set.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        return _set.SetEquals(other);
    }

    public void ExceptWith(IEnumerable<T> other)
    {
        _set.ExceptWith(other);
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        _set.IntersectWith(other);
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        _set.SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<T> other)
    {
        _set.UnionWith(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueSet<T> readOnlyValueSet && Equals(readOnlyValueSet);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var item in _set)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }

    public virtual bool Equals(ValueSet<T>? other)
    {
        return other is not null && SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_set).GetEnumerator();
    }
}
