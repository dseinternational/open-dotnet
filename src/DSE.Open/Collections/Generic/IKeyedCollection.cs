// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Collections.Generic;

public interface IKeyedCollection<TKey, TItem>
    : IList<TItem>, IList, IReadOnlyList<TItem>
    where TKey : notnull
{
    TItem this[TKey key] { get; }
}
