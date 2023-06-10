// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// An observable collection that represents a group of items.
/// </summary>
/// <typeparam name="TGroup">The type representing the group.</typeparam>
/// <typeparam name="TItem">The type of the items.</typeparam>
/// <typeparam name="TGrouping"></typeparam>
[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "By design")]
public class ObservableGroupCollection<TGroup, TItem, TGrouping>
    : ObservableCollection<TGrouping>, IGroupCollection<TGroup, TItem, TGrouping>
    where TGrouping : IGrouping<TGroup, TItem>
{
    public ObservableGroupCollection()
    {
    }

    public ObservableGroupCollection(IEnumerable<TGrouping> collection) : base(collection)
    {
    }
}
