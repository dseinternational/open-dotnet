// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": data } ]

public sealed class ValueLabelCollection<T> : ValueLabelCollectionBase<T>, IValueLabelCollection<T>
    where T : IEquatable<T>
{
    public ValueLabelCollection() : this([])
    {
    }

    public ValueLabelCollection(IEnumerable<ValueLabel<T>> dataLabels) : base([.. dataLabels])
    {
    }

    public string this[T data]
    {
        get
        {
            if (ValueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return ValueLabels[ValueIndexLookup[GetHash(data)]].Label;
        }
        set => Add(new ValueLabel<T>(data, value));
    }

    bool ICollection<ValueLabel<T>>.IsReadOnly => false;

    public void Add(ValueLabel<T> label)
    {
        AddCore(label);
    }

    public void Add(T data, string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        Add((data, label));
    }

    public ReadOnlyValueLabelCollection<T> AsReadOnly()
    {
        return new ReadOnlyValueLabelCollection<T>(ValueLabels);
    }

    public void Clear()
    {
        ClearCore();
    }

    void ICollection<ValueLabel<T>>.CopyTo(ValueLabel<T>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < ValueLabels.Count)
        {
            throw new ArgumentException(
                "The destination array is not long enough to copy all the items in the collection.");
        }

        for (var i = 0; i < ValueLabels.Count; i++)
        {
            array[arrayIndex + i] = ValueLabels[i];
        }
    }

    public bool Remove(ValueLabel<T> dataLabel)
    {
        return RemoveCore(dataLabel);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
