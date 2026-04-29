// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Represents an observable list that raises change notifications and supports replacing all items in one operation.
/// </summary>
/// <typeparam name="T">The type of element stored in the list.</typeparam>
public interface IObservableList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    /// <summary>
    /// Replaces the contents of the list with <paramref name="items"/>, raising change notifications as appropriate.
    /// </summary>
    void SetRange(IEnumerable<T> items);
}
