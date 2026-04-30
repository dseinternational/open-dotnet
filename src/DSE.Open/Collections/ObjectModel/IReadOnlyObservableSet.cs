// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Represents a read-only observable set that raises change notifications.
/// </summary>
/// <typeparam name="T">The type of element stored in the set.</typeparam>
public interface IReadOnlyObservableSet<T> : IReadOnlySet<T>, INotifyCollectionChanged, INotifyPropertyChanged;
