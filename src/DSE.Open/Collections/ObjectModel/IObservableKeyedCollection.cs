// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Collections.ObjectModel;

public interface IObservableKeyedCollection<TKey, TItem> : IKeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged
    where TKey : notnull;
