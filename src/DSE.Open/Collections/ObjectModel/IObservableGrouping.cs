// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

#pragma warning disable CA1710 // Identifiers should have correct suffix
/// <summary>
/// Represents a mutable, observable grouping of items by a key.
/// </summary>
/// <typeparam name="TGroup">The type of the group key.</typeparam>
/// <typeparam name="TItem">The type of items in the group.</typeparam>
public interface IObservableGrouping<TGroup, TItem>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    : IGrouping<TGroup, TItem>, ICollection<TItem>, INotifyCollectionChanged, INotifyPropertyChanged
{
    /// <summary>
    /// Gets the key of the group.
    /// </summary>
    TGroup Group { get; }
}
