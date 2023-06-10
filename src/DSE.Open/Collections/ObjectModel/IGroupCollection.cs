// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Represents a collection of grouped collections of items.
/// </summary>
/// <typeparam name="TGroup">The type presenting the grouping</typeparam>
/// <typeparam name="TItem">The type of the grouped items</typeparam>
/// <typeparam name="TGrouping"></typeparam>
[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "By design")]
public interface IGroupCollection<out TGroup, out TItem, TGrouping>
    : IList<TGrouping>
    where TGrouping : IGrouping<TGroup, TItem>
{
}

public interface IGroupCollection<TGroup, TItem>
    : IList<IGrouping<TGroup, TItem>>
{
}
