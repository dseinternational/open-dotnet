// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

/// <summary>
/// Provides extension methods for converting sequences to observable collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="ObservableList{T}"/> containing the elements of <paramref name="values"/>.
    /// </summary>
    public static ObservableList<T> ToObservableList<T>(this IEnumerable<T> values)
    {
        return [.. values];
    }
}
