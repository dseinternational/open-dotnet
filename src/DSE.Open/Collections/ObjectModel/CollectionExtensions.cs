// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

public static class CollectionExtensions
{
    public static ObservableList<T> ToObservableList<T>(this IEnumerable<T> values)
    {
        return new(values);
    }
}
