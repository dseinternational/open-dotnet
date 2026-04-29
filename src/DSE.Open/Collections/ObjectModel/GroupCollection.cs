// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// A collection of <typeparamref name="TGrouping"/> values that group items of type <typeparamref name="TItem"/> by a key of type <typeparamref name="TGroup"/>.
/// </summary>
/// <typeparam name="TGroup">The type of the group key.</typeparam>
/// <typeparam name="TItem">The type of the items in each group.</typeparam>
/// <typeparam name="TGrouping">The grouping type implementing <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "By design")]
public class GroupCollection<TGroup, TItem, TGrouping>
    : Collection<TGrouping>, IGroupCollection<TGroup, TItem, TGrouping>
    where TGrouping : IGrouping<TGroup, TItem>
{
    /// <summary>
    /// Initializes a new, empty <see cref="GroupCollection{TGroup, TItem, TGrouping}"/>.
    /// </summary>
    public GroupCollection()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="GroupCollection{TGroup, TItem, TGrouping}"/> containing the specified groupings.
    /// </summary>
    public GroupCollection(IEnumerable<TGrouping> list) : base(list.ToList())
    {
    }

    /// <summary>
    /// Initializes a new <see cref="GroupCollection{TGroup, TItem, TGrouping}"/> wrapping the specified list of groupings.
    /// </summary>
    public GroupCollection(IList<TGrouping> list) : base(list)
    {
    }
}

/// <summary>
/// A collection of <see cref="IGrouping{TKey, TElement}"/> values grouping items of type <typeparamref name="TItem"/> by a key of type <typeparamref name="TGroup"/>.
/// </summary>
/// <typeparam name="TGroup">The type of the group key.</typeparam>
/// <typeparam name="TItem">The type of the items in each group.</typeparam>
public class GroupCollection<TGroup, TItem>
    : GroupCollection<TGroup, TItem, IGrouping<TGroup, TItem>>, IGroupCollection<TGroup, TItem>
{
    /// <summary>
    /// Initializes a new, empty <see cref="GroupCollection{TGroup, TItem}"/>.
    /// </summary>
    public GroupCollection()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="GroupCollection{TGroup, TItem}"/> containing the specified groupings.
    /// </summary>
    public GroupCollection(IEnumerable<IGrouping<TGroup, TItem>> list) : base(list)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="GroupCollection{TGroup, TItem}"/> wrapping the specified list of groupings.
    /// </summary>
    public GroupCollection(IList<IGrouping<TGroup, TItem>> list) : base(list)
    {
    }
}
