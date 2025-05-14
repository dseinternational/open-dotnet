// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": data } ]

public sealed class DataLabelCollection<TData> : DataLabelCollectionBase<TData>, IDataLabelCollection<TData>
    where TData : IEquatable<TData>
{
    public DataLabelCollection() : this([])
    {
    }

    public DataLabelCollection(IEnumerable<DataLabel<TData>> dataLabels) : base([.. dataLabels])
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
        set => Add(new DataLabel<TData>(data, value));
    }

    bool ICollection<DataLabel<TData>>.IsReadOnly => false;

    public void Add(DataLabel<TData> label)
    {
        AddCore(label);
    }

    public void Add(TData data, string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        Add((data, label));
    }

    public void Clear()
    {
        ClearCore();
    }

    void ICollection<DataLabel<TData>>.CopyTo(DataLabel<TData>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < DataLabels.Count)
        {
            throw new ArgumentException(
                "The destination array is not long enough to copy all the items in the collection.");
        }

        for (var i = 0; i < DataLabels.Count; i++)
        {
            array[arrayIndex + i] = DataLabels[i];
        }
    }

    public bool Remove(DataLabel<TData> dataLabel)
    {
        return RemoveCore(dataLabel);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
