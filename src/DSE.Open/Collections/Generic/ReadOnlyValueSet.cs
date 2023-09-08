// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A <see cref="IReadOnlySet{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks><see cref="ICollection{T}"/> is implemented explicitly to support deserialization.</remarks>
public class ReadOnlyValueSet<T> : IReadOnlySet<T>, IEquatable<ReadOnlyValueSet<T>>, ICollection<T>
{
    public static readonly ReadOnlyValueSet<T> Empty = new(Enumerable.Empty<T>());

    private readonly HashSet<T> _set;

    public ReadOnlyValueSet()
    {
        _set = new HashSet<T>();
    }

    public ReadOnlyValueSet(IEnumerable<T> set)
    {
        Guard.IsNotNull(set);

        _set = new HashSet<T>(set);
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

    public bool Equals(ReadOnlyValueSet<T>? other)
    {
        return other is not null && SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_set).GetEnumerator();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyValueSet<T>(T[] collection) => new(collection);

    public static explicit operator ReadOnlyValueSet<T>(ReadOnlyCollection<T> collection) => new(collection);

    public static explicit operator ReadOnlyValueSet<T>(Collection<T> collection) => new(collection);

    public static explicit operator ReadOnlyValueSet<T>(HashSet<T> collection) => new(collection);

#pragma warning restore CA2225 // Operator overloads have named alternates
}
