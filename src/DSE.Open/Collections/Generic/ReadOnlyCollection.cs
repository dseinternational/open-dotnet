// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public static class ReadOnlyCollection
{
    /// <summary>
    /// Creates an <see cref="ReadOnlyCollection{T}"/> with the specified elements.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the collection.</typeparam>
    /// <param name="items">The elements to store in the collection.</param>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/> containing the specified items.</returns>
    public static ReadOnlyCollection<T> Create<T>(ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
        {
            return [];
        }

        var list = new List<T>(items.Length);
        list.AddRange(items);

        return CreateUnsafe(list);
    }

    /// <summary>
    /// Creates an <see cref="ReadOnlyCollection{T}"/> with the specified elements.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the collection.</typeparam>
    /// <param name="items">The elements to store in the collection.</param>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/> containing the specified items.</returns>
    public static ReadOnlyCollection<T> Create<T>(Span<T> items)
    {
        return Create((ReadOnlySpan<T>)items);
    }

    /// <summary>
    /// Creates an <see cref="ReadOnlyCollection{T}"/> populated with the contents of the specified sequence.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the collection.</typeparam>
    /// <param name="items">The elements to store in the collection.</param>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/>.</returns>
    public static ReadOnlyCollection<T> CreateRange<T>(IEnumerable<T> items)
    {
        if (items is ReadOnlyCollection<T> r)
        {
            // If the provided instance is a `ReadOnlyCollection<T>`, then the underlying backing store can be shared.
            return CreateUnsafe(r._items);
        }

        if (items is ReadOnlyValueCollection<T> rv)
        {
            return CreateUnsafe(rv._items);
        }

        return CreateUnsafe(items.ToList());
    }

    /// <summary>
    /// Unsafely creates a <see cref="ReadOnlyCollection{T}"/> using the provided list as it's backing store.
    /// This should only be done when the caller is the only holder of the list, and does not mutate it after constructing this collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
#pragma warning disable CA1002 // Do not expose generic lists
    public static ReadOnlyCollection<T> CreateUnsafe<T>(List<T> items)
    {
        return new(items);
    }
#pragma warning restore CA1002 // Do not expose generic lists
}
