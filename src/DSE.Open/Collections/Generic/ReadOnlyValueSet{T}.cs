// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A <see cref="IReadOnlySet{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks><see cref="ICollection{T}"/> is implemented explicitly to support deserialization.</remarks>
[CollectionBuilder(typeof(ReadOnlyValueSet), nameof(ReadOnlyValueSet.Create))]
public class ReadOnlyValueSet<T> : IReadOnlySet<T>, IEquatable<ReadOnlyValueSet<T>>, ICollection<T>
{
    public static readonly ReadOnlyValueSet<T> Empty = new(Enumerable.Empty<T>());
#pragma warning restore IDE0306 // Simplify collection initialization

    private readonly HashSet<T> _set;

    public ReadOnlyValueSet() : this([])
    {
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal ReadOnlyValueSet(HashSet<T> set)
    {
        Debug.Assert(set is not null);
        _set = set;
    }

    /// <summary>
    /// Provides a method for derived types to directly set the backing store. <paramref name="noCopy"/> must be <c>true</c>.
    /// </summary>
    /// <param name="set">The set to use as the backing store.</param>
    /// <param name="noCopy">Must be true.</param>
    protected ReadOnlyValueSet(HashSet<T> set, bool noCopy)
    {
        Debug.Assert(noCopy);
        _set = set;
    }

    public ReadOnlyValueSet(IEnumerable<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set is ReadOnlyValueSet<T> other)
        {
            _set = other._set;
        }
        else
        {
            _set = [.. set];
        }
    }

    public int Count => _set.Count;

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<T>.IsReadOnly => false; // JsonSerialization

    void ICollection<T>.Add(T item)
    {
        _ = _set.Add(item); // JsonSerialization
    }

    void ICollection<T>.Clear()
    {
        _set.Clear(); // JsonSerialization
    }

    bool ICollection<T>.Remove(T item)
    {
        ThrowHelper.ThrowNotSupportedException("Cannot remove items from a read-only collection.");
        return false;
    }

#pragma warning restore CA1033 // Interface methods should be callable by child types

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

    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyValueSet<T> readOnlyValueSet && Equals(readOnlyValueSet);
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

    public virtual bool Equals(ReadOnlyValueSet<T>? other)
    {
        return other is not null && SetEquals(other);
    }

    public override string ToString()
    {
        return CollectionWriter.WriteToString(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_set).GetEnumerator();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyValueSet<T>(T[] collection)
    {
        return [.. collection];
    }

    public static explicit operator ReadOnlyValueSet<T>(ReadOnlyCollection<T> collection)
    {
        return [.. collection];
    }

    public static explicit operator ReadOnlyValueSet<T>(Collection<T> collection)
    {
        return [.. collection];
    }

    public static explicit operator ReadOnlyValueSet<T>(System.Collections.ObjectModel.ReadOnlyCollection<T> collection)
    {
        return [.. collection];
    }

    public static explicit operator ReadOnlyValueSet<T>(System.Collections.ObjectModel.Collection<T> collection)
    {
        return [.. collection];
    }

    public static explicit operator ReadOnlyValueSet<T>(HashSet<T> collection)
    {
        return [.. (IEnumerable<T>)collection];
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
