// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class Collection<T> : System.Collections.ObjectModel.Collection<T>
{
    public static readonly Collection<T> Empty = [];

    public Collection()
    {
    }

    public Collection(int count)
        : base(new List<T>(count))
    {
    }

    public Collection(IEnumerable<T> collection)
        : base(collection is IList<T> list ? list : new List<T>(collection))
    {
    }

    public Collection(IList<T> list) : base(list)
    {
    }
}
