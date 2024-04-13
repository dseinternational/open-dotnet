// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A set of initialization methods for instances of <see cref="ReadOnlyValueSet{T}"/>.
/// </summary>
public static class ReadOnlyValueSet
{
    /// <summary>
    /// Creates an <see cref="ReadOnlyValueSet{T}"/> with the specified elements.
    /// </summary>
    /// <typeparam name="T">The type of element stored in the set.</typeparam>
    /// <param name="items">The elements to store in the set.</param>
    /// <returns>A <see cref="ReadOnlyValueSet{T}"/> containing the specified items.</returns>
    public static ReadOnlyValueSet<T> Create<T>(ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
        {
            return [];
        }

        var set = new HashSet<T>(items.Length);

        foreach (var item in items)
        {
            _ = set.Add(item);
        }

        return new ReadOnlyValueSet<T>(set);
    }

}
