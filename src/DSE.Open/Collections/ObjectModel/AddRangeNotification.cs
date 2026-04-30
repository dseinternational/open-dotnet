// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Specifies how an <c>AddRange</c> operation reports its change via <see cref="System.Collections.Specialized.INotifyCollectionChanged"/>.
/// </summary>
public enum AddRangeNotification
{
    /// <summary>
    /// Raises a single <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Add"/> event with the added items.
    /// </summary>
    Add,

    /// <summary>
    /// Raises a <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/> event after adding the items.
    /// </summary>
    Reset
}
