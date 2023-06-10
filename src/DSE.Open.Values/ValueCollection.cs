// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Values;

public class ValueCollection<TValue, T> : Collection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public ValueCollection()
    {
    }

    public ValueCollection(IEnumerable<TValue> collection) : base(collection.ToList())
    {
    }
}