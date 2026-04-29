// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Represents an observable set that raises change and property-change/changing notifications.
/// </summary>
/// <typeparam name="T">The type of element stored in the set.</typeparam>
public interface IObservableSet<T> : ISet<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyPropertyChanging;
