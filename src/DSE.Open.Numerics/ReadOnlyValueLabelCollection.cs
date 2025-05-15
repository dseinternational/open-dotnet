// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": value } ]

public sealed class ReadOnlyValueLabelCollection<TData>
    : ValueLabelCollectionBase<TData>,
      IReadOnlyValueLabelCollection<TData>
    where TData : IEquatable<TData>
{
    public ReadOnlyValueLabelCollection() : this([])
    {
    }

    public ReadOnlyValueLabelCollection(IEnumerable<ValueLabel<TData>> valueLabels) : base([.. valueLabels])
    {
    }

    public string this[TData value]
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
