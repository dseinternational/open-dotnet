// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Collections.ObjectModel;

[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "By design")]
public class GroupCollection<TGroup, TItem, TGrouping>
    : Collection<TGrouping>, IGroupCollection<TGroup, TItem, TGrouping>
    where TGrouping : IGrouping<TGroup, TItem>
{
    public GroupCollection()
    {
    }

    public GroupCollection(IEnumerable<TGrouping> list) : base(list.ToList())
    {
    }

    public GroupCollection(IList<TGrouping> list) : base(list)
    {
    }
}

public class GroupCollection<TGroup, TItem>
    : GroupCollection<TGroup, TItem, IGrouping<TGroup, TItem>>, IGroupCollection<TGroup, TItem>
{
    public GroupCollection()
    {
    }

    public GroupCollection(IEnumerable<IGrouping<TGroup, TItem>> list) : base(list)
    {
    }

    public GroupCollection(IList<IGrouping<TGroup, TItem>> list) : base(list)
    {
    }
}
