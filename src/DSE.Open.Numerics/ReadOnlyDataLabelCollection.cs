// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": data } ]

public sealed class ReadOnlyDataLabelCollection<TData>
    : DataLabelCollectionBase<TData>,
      IReadOnlyDataLabelCollection<TData>
    where TData : IEquatable<TData>
{
    public ReadOnlyDataLabelCollection() : this([])
    {
    }

    public ReadOnlyDataLabelCollection(IEnumerable<DataLabel<TData>> dataLabels) : base([.. dataLabels])
    {
    }

    public string this[TData data]
    {
        get
        {
            if (DataLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return DataLabels[DataIndexLookup[GetHash(data)]].Label;
        }
    }
}
