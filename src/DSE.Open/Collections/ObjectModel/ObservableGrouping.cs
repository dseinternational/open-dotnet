// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

public class ObservableGrouping<TGroup, TItem> : ObservableCollection<TItem>, IObservableGrouping<TGroup, TItem>
{
    private TGroup _group;

    /// <summary>
    /// Initializes a new instance with the provided group value and no items.
    /// </summary>
    /// <param name="group">A value identifying the group.</param>
    public ObservableGrouping(TGroup group) : this(group, Array.Empty<TItem>())
    {
    }

    /// <summary>
    /// Initializes a new instance with the provided grouping, with the grouping key identifying
    /// the group and elements copied from the specified collection.
    /// </summary>
    /// <param name="grouping">The grouping to initialize the collection with.</param>
    public ObservableGrouping(IGrouping<TGroup, TItem> grouping)
        : this(grouping is not null ? grouping.Key : throw new ArgumentNullException(nameof(grouping)), grouping)
    {
    }

    /// <summary>
    /// Initializes a new instance with the provided group value and elements copied from
    /// the specified collection.
    /// </summary>
    /// <param name="group">A value identifying the group.</param>
    /// <param name="collection"></param>
    public ObservableGrouping(TGroup group, IEnumerable<TItem> collection) : base(collection)
    {
        _group = group;
    }

    /// <summary>
    /// Gets the group shared by the items in the collection.
    /// </summary>
    public TGroup Group
    {
        get => _group;
        set
        {
            if (!EqualityComparer<TGroup>.Default.Equals(value, _group))
            {
                _group = value;
                OnPropertyChanged(new(nameof(Group)));
            }
        }
    }

    /// <summary>
    /// Gets the group shared by the items in the collection.
    /// </summary>
#pragma warning disable CA1033 // Interface methods should be callable by child types
    TGroup IGrouping<TGroup, TItem>.Key => Group;
#pragma warning restore CA1033 // Interface methods should be callable by child types

}
