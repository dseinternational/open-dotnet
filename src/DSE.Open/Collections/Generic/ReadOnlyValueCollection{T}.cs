// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A <see cref="IReadOnlyCollection{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks><see cref="ICollection{T}"/> is implemented explicitly to support deserialization.</remarks>
[CollectionBuilder(typeof(ReadOnlyValueCollection), nameof(ReadOnlyValueCollection.Create))]
public class ReadOnlyValueCollection<T>
    : IReadOnlyList<T>,
      ICollection<T>,
      IEquatable<ReadOnlyValueCollection<T>>
{
    // Cannot use collection expression because this is the empty used by the builder when supplied with no items.
    public static readonly ReadOnlyValueCollection<T> Empty = new();

    internal readonly List<T> _items;

    public ReadOnlyValueCollection()
    {
        _items = [];
    }

    public ReadOnlyValueCollection(IEnumerable<T> list)
    {
        _items = list is ReadOnlyValueCollection<T> rovc ? rovc._items : [.. list];
    }

    /// <summary>
    /// Unsafely creates a <see cref="ReadOnlyValueCollection{T}"/> using the provided list as it's backing store.
    /// This should only be done when the caller is the only holder of the list, and does not mutate it after constructing this collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal ReadOnlyValueCollection(List<T> items)
    {
        _items = items;
    }

    /// <summary>
    /// Provides a method for derived types to directly set the backing store. <paramref name="noCopy"/> must be <c>true</c>.
    /// </summary>
    /// <param name="items">The list to use as the backing store.</param>
    /// <param name="noCopy">Must be true.</param>
#pragma warning disable CA1002 // Do not expose generic lists
    protected ReadOnlyValueCollection(List<T> items, bool noCopy)
    {
        Debug.Assert(noCopy);
        _items = items;
    }
#pragma warning restore CA1002 // Do not expose generic lists

    public int Count => _items.Count;

    public T this[int index] => _items[index];

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<T>.IsReadOnly => false; // JsonSerialization

    public ReadOnlySpan<T> AsSpan()
    {
        return CollectionsMarshal.AsSpan(_items);
    }

    void ICollection<T>.Add(T item)
    {
        _items.Add(item); // JsonSerialization
    }

    void ICollection<T>.Clear()
    {
        _items.Clear(); // JsonSerialization
    }

    bool ICollection<T>.Remove(T item)
    {
        ThrowHelper.ThrowNotSupportedException("Cannot remove items from a read-only collection.");
        return false;
    }

#pragma warning restore CA1033 // Interface methods should be callable by child types

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public virtual bool Equals(ReadOnlyValueCollection<T>? other)
    {
        return other is not null
            && (ReferenceEquals(this, other)
                || (Count == other.Count
                    && CollectionsMarshal.AsSpan(_items).SequenceEqual(CollectionsMarshal.AsSpan(other._items))));
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ReadOnlyValueCollection<T>);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyValueCollection<T>(T[] collection)
    {
        return new ReadOnlyValueCollection<T>(collection);
    }

    public static explicit operator ReadOnlyValueCollection<T>(ReadOnlyCollection<T> collection)
    {
        return new ReadOnlyValueCollection<T>(collection);
    }

    public static explicit operator ReadOnlyValueCollection<T>(Collection<T> collection)
    {
        return new ReadOnlyValueCollection<T>(collection);
    }

    public static explicit operator ReadOnlyValueCollection<T>(HashSet<T> collection)
    {
        return new ReadOnlyValueCollection<T>(collection);
    }
#pragma warning restore CA2225 // Operator overloads have named alternates

    public struct Enumerator : IEnumerator<T>
    {
        private List<T>.Enumerator _inner;

        public Enumerator(ReadOnlyValueCollection<T> collection)
        {
            Guard.IsNotNull(collection);
            _inner = collection._items.GetEnumerator();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            ((IEnumerator)_inner).Reset();
        }

        public T Current => _inner.Current;

        object? IEnumerator.Current => _inner.Current;

        public void Dispose()
        {
            _inner.Dispose();
        }
    }
}
