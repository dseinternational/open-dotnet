// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

public sealed class DataLabelCollection<TData, TLabel> : IDataLabelCollection<TData, TLabel>
    where TData : IEquatable<TData>
    where TLabel : IEquatable<TLabel>
{
    // we cannot use a Dictionary here because we may want to support null TData

    private readonly HashSet<TData> _dataHashset;
    private readonly Collection<TData> _data;
    private readonly Collection<TLabel> _labels;

    // todo: consider Dictionary<int, DataLabel<TData, TLabel>> where int is the hashcode of TData

    public DataLabelCollection()
    {
        _dataHashset = [];
        _data = [];
        _labels = [];
    }

    public DataLabelCollection(IEnumerable<DataLabel<TData, TLabel>> dataLabels)
    {
        ArgumentNullException.ThrowIfNull(dataLabels);

        if (dataLabels is IReadOnlyCollection<DataLabel<TData, TLabel>> dataLabelCol)
        {
            _dataHashset = new HashSet<TData>(dataLabelCol.Count);
            _data = new Collection<TData>(dataLabelCol.Count);
            _labels = new Collection<TLabel>(dataLabelCol.Count);
        }
        else
        {
            _dataHashset = [];
            _data = [];
            _labels = [];
        }

        foreach (var label in dataLabels)
        {
            Add(label);
        }
    }

    public TLabel this[TData data]
    {
        get
        {
            var index = _data.IndexOf(data);

            if (index < 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return _labels[index];
        }
        set => Add(new DataLabel<TData, TLabel>(data, value));
    }

    public int Count => _data.Count;

    bool ICollection<DataLabel<TData, TLabel>>.IsReadOnly => false;

    public void Add(DataLabel<TData, TLabel> label)
    {
        Add(label.Data, label.Label);
    }

    public void Add(TData data, TLabel label)
    {
        if (_dataHashset.Add(data))
        {
            _data.Add(data);
            _labels.Add(label);
        }
        else
        {
            var index = _data.IndexOf(data);

            if (index >= 0)
            {
                _labels[index] = label;
            }
        }

        Debug.Assert(_data.Count == _labels.Count, "Data and labels count mismatch.");
        Debug.Assert(_dataHashset.Count == _data.Count, "Data hashset and data count mismatch.");
    }

    public void Clear()
    {
        _dataHashset.Clear();
        _data.Clear();
        _labels.Clear();
    }

    public bool Contains(DataLabel<TData, TLabel> item)
    {
        if (!_dataHashset.Contains(item.Data))
        {
            return false;
        }

        var index = _data.IndexOf(item.Data);

        Debug.Assert(index >= 0, "Data not found in collection.");

        return _labels[index].Equals(item.Label);

    }

    void ICollection<DataLabel<TData, TLabel>>.CopyTo(DataLabel<TData, TLabel>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < _data.Count)
        {
            throw new ArgumentException(
                "The destination array is not long enough to copy all the items in the collection.");
        }

        for (var i = 0; i < _data.Count; i++)
        {
            array[arrayIndex + i] = new DataLabel<TData, TLabel>(_data[i], _labels[i]);
        }
    }

    public IEnumerator<DataLabel<TData, TLabel>> GetEnumerator()
    {
        for (var i = 0; i < _data.Count; i++)
        {
            yield return new DataLabel<TData, TLabel>(_data[i], _labels[i]);
        }
    }

    public bool Remove(DataLabel<TData, TLabel> item)
    {
        if (!_dataHashset.Remove(item.Data))
        {
            return false;
        }

        var index = _data.IndexOf(item.Data);

        Debug.Assert(index >= 0, "Data not found in collection.");

        _data.RemoveAt(index);
        _labels.RemoveAt(index);

        Debug.Assert(_data.Count == _labels.Count, "Data and labels count mismatch.");
        Debug.Assert(_dataHashset.Count == _data.Count, "Data hashset and data count mismatch.");

        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
