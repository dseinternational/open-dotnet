// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

public interface IObservableSet<T> : ISet<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyPropertyChanging;
