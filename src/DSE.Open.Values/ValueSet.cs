// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public class ValueSet<TValue, T> : HashSet<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    public ValueSet()
    {
    }

    public ValueSet(IEnumerable<TValue> collection) : base(collection)
    {
    }

    public ValueSet(IEqualityComparer<TValue>? comparer) : base(comparer)
    {
    }

    public ValueSet(int capacity) : base(capacity)
    {
    }

    public ValueSet(IEnumerable<TValue> collection, IEqualityComparer<TValue>? comparer) : base(collection, comparer)
    {
    }

    public ValueSet(int capacity, IEqualityComparer<TValue>? comparer) : base(capacity, comparer)
    {
    }
}