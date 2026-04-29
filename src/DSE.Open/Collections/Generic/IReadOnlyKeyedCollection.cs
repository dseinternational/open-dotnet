// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Represents a read-only list of items that can also be retrieved by an associated key.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TItem">The item type.</typeparam>
public interface IReadOnlyKeyedCollection<TKey, TItem>
    : IList<TItem>, System.Collections.IList, IReadOnlyList<TItem>
    where TKey : notnull
{
    /// <summary>
    /// Gets the item associated with the specified key.
    /// </summary>
    TItem this[TKey key] { get; }
}
