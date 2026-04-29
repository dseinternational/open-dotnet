// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Provides extension methods for <see cref="IObservableList{T}"/>.
/// </summary>
public static class ObservableListExtensions
{
    /// <summary>
    /// Get a read-only view of the observable list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static ReadOnlyObservableList<T> AsReadOnlyObservableList<T>(this IObservableList<T> list)
    {
        return new ReadOnlyObservableList<T>(list);
    }
}
