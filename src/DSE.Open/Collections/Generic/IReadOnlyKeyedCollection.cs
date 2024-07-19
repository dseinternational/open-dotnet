// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public interface IReadOnlyKeyedCollection<TKey, TItem>
    : IList<TItem>, System.Collections.IList, IReadOnlyList<TItem>
    where TKey : notnull
{
    TItem this[TKey key] { get; }
}
