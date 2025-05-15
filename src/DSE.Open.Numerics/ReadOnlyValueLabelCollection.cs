// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": value } ]

public static class ReadOnlyValueLabelCollection
{
    public static ReadOnlyValueLabelCollection<T> Create<T>(ReadOnlySpan<ValueLabel<T>> span)
        where T : IEquatable<T>
    {
        var labels = new ValueLabel<T>[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            labels[i] = span[i];
        }

        return new ReadOnlyValueLabelCollection<T>(labels);
    }
}

[CollectionBuilder(typeof(ReadOnlyValueLabelCollection), nameof(ReadOnlyValueLabelCollection.Create))]
public sealed class ReadOnlyValueLabelCollection<T>
    : ValueLabelCollectionBase<T>,
      IReadOnlyValueLabelCollection<T>
    where T : IEquatable<T>
{
    public ReadOnlyValueLabelCollection() : this([])
    {
    }

    public ReadOnlyValueLabelCollection(IEnumerable<ValueLabel<T>> valueLabels) : this([.. valueLabels])
    {
    }

    internal ReadOnlyValueLabelCollection(Collection<ValueLabel<T>> valueLabels) : base(valueLabels)
    {
    }

    public string this[T value]
    {
        get
        {
            if (ValueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Value {value} not found.");
            }

            return ValueLabels[ValueIndexLookup[GetHash(value)]].Label;
        }
    }
}
