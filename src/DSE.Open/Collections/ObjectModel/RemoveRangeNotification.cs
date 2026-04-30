// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Specifies how a <c>RemoveRange</c> operation reports its change via <see cref="System.Collections.Specialized.INotifyCollectionChanged"/>.
/// </summary>
public enum RemoveRangeNotification
{
    /// <summary>
    /// Raises a <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/> event after removing the items.
    /// </summary>
    Reset,

    /// <summary>
    /// Raises a single <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Remove"/> event with the removed items.
    /// </summary>
    Remove
}
