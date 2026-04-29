// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Represents a list of items that can also be retrieved by an associated key.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TItem">The item type.</typeparam>
public interface IKeyedCollection<TKey, TItem>
    : IList<TItem>, IList, IReadOnlyList<TItem>, IReadOnlyKeyedCollection<TKey, TItem>
    where TKey : notnull
{
}
