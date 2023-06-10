// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A <see cref="IReadOnlyCollection{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks><see cref="ICollection{T}"/> is implemented explicitly to support deserialization.</remarks>
public class ReadOnlyValueCollection<T>
    : IReadOnlyList<T>,
      ICollection<T>,
      IEquatable<ReadOnlyValueCollection<T>>
    where T
    : IEquatable<T>
{
    public static readonly ReadOnlyValueCollection<T> Empty = new();

    private readonly List<T> _items;

    public ReadOnlyValueCollection()
    {
        _items = new List<T>();
    }

    public ReadOnlyValueCollection(IEnumerable<T> list)
    {
        _items = list is ReadOnlyValueCollection<T> rovc ? rovc._items : new List<T>(list);
    }

    public int Count => _items.Count;

    public T this[int index] => _items[index];

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<T>.IsReadOnly => false; // JsonSerialization

    void ICollection<T>.Add(T item) => _items.Add(item); // JsonSerialization

    void ICollection<T>.Clear() => _items.Clear(); // JsonSerialization

#pragma warning restore CA1033 // Interface methods should be callable by child types

    public bool Contains(T item) => _items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
        => _items.CopyTo(array, arrayIndex);

    public bool Equals(ReadOnlyValueCollection<T>? other)
        => other is not null
            && (ReferenceEquals(this, other) || (Count == other.Count && this.SequenceEqual(other)));

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyValueCollection<T>);

    public override int GetHashCode()
    {
        var hashCode = -7291863;

        foreach (var i in this)
        {
            hashCode = HashCode.Combine(hashCode, i);
        }

        return hashCode;
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

#pragma warning disable CA1033 // Interface methods should be callable by child types
    bool ICollection<T>.Remove(T item)
    {
        ThrowHelper.ThrowNotSupportedException("Cannot remove items from a read-only collection.");
        return false;
    }
#pragma warning restore CA1033 // Interface methods should be callable by child types

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyValueCollection<T>(T[] collection) => new(collection);

    public static explicit operator ReadOnlyValueCollection<T>(ReadOnlyCollection<T> collection) => new(collection);

    public static explicit operator ReadOnlyValueCollection<T>(Collection<T> collection) => new(collection);

    public static explicit operator ReadOnlyValueCollection<T>(HashSet<T> collection) => new(collection);

#pragma warning restore CA2225 // Operator overloads have named alternates
}
