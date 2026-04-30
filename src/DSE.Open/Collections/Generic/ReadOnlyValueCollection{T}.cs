// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Collections.Generic;

/// <summary>
///     A <see cref="IReadOnlyCollection{T}"/> that defines equality based on the equality of the items it contains.
/// </summary>
/// <typeparam name="T">The type of element stored in the collection.</typeparam>
/// <remarks>
///     <see cref="ICollection{T}"/> is implemented explicitly to support deserialization and <see cref="IList"/>
///     is implemented to support certain data-binding scenarios.
/// </remarks>
[CollectionBuilder(typeof(ReadOnlyValueCollection), nameof(ReadOnlyValueCollection.Create))]
public class ReadOnlyValueCollection<T>
    : IReadOnlyList<T>,
      ICollection<T>,
      IList,
      IEquatable<ReadOnlyValueCollection<T>>
{
    /// <summary>
    /// An empty <see cref="ReadOnlyValueCollection{T}"/>.
    /// </summary>
    // Cannot use collection expression because this is the empty used by the builder when supplied with no items.
    public static readonly ReadOnlyValueCollection<T> Empty = new();

    internal readonly List<T> _items;

    /// <summary>
    /// Initializes a new, empty <see cref="ReadOnlyValueCollection{T}"/>.
    /// </summary>
    public ReadOnlyValueCollection()
    {
        _items = [];
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyValueCollection{T}"/> containing the elements of <paramref name="list"/>.
    /// If <paramref name="list"/> is itself a <see cref="ReadOnlyValueCollection{T}"/>, its backing store is shared.
    /// </summary>
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

    /// <inheritdoc/>
    public int Count => _items.Count;

    /// <inheritdoc/>
    public T this[int index] => _items[index];

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="item"/>, or -1 if not found.
    /// </summary>
    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    bool ICollection<T>.IsReadOnly => false; // JsonSerialization

    bool IList.IsFixedSize => true;

    bool IList.IsReadOnly => true;

    bool ICollection.IsSynchronized => ((IList)_items).IsSynchronized;

    object ICollection.SyncRoot => ((IList)_items).SyncRoot;

    object? IList.this[int index] { get => this[index]; set => throw new InvalidOperationException("Cannot change a read-only collection."); }

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> over the underlying storage of the collection.
    /// </summary>
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

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public virtual bool Equals(ReadOnlyValueCollection<T>? other)
    {
        return other is not null
            && (ReferenceEquals(this, other)
                || (Count == other.Count
                    && CollectionsMarshal.AsSpan(_items).SequenceEqual(CollectionsMarshal.AsSpan(other._items))));
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ReadOnlyValueCollection<T>);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new(this);
    }

    /// <summary>
    /// Returns a string representation of the collection produced by <see cref="CollectionWriter"/>.
    /// </summary>
    public override string ToString()
    {
        return CollectionWriter.WriteToString(this);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    int IList.Add(object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.Clear()
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    bool IList.Contains(object? value)
    {
        if (value is not T item)
        {
            return false;
        }

        return Contains(item);
    }

    int IList.IndexOf(object? value)
    {
        if (value is not T item)
        {
            return -1;
        }

        return IndexOf(item);
    }

    void IList.Insert(int index, object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.Remove(object? value)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void IList.RemoveAt(int index)
    {
        throw new InvalidOperationException("Cannot change a read-only collection.");
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((IList)_items).CopyTo(array, index);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified array.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(T[] collection)
    {
        return [.. collection];
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(ReadOnlyCollection<T> collection)
    {
        return [.. collection];
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(Collection<T> collection)
    {
        return [.. collection];
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(System.Collections.ObjectModel.ReadOnlyCollection<T> collection)
    {
        return [.. collection];
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified collection.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(System.Collections.ObjectModel.Collection<T> collection)
    {
        return [.. collection];
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlyValueCollection{T}"/> containing a copy of the specified hash set.
    /// </summary>
    public static explicit operator ReadOnlyValueCollection<T>(HashSet<T> collection)
    {
        return [.. collection];
    }
#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// An enumerator over a <see cref="ReadOnlyValueCollection{T}"/>.
    /// </summary>
    public struct Enumerator : IEnumerator<T>
    {
        private List<T>.Enumerator _inner;

        /// <summary>
        /// Initializes a new enumerator that iterates the specified collection.
        /// </summary>
        public Enumerator(ReadOnlyValueCollection<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);
            _inner = collection._items.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public T Current => _inner.Current;

        object? IEnumerator.Current => _inner.Current;

        /// <inheritdoc/>
        public void Dispose()
        {
            _inner.Dispose();
        }
    }
}
