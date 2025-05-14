// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": data } ]

public sealed class ReadOnlyDataLabelCollection<TData>
    : DataLabelCollectionBase<TData>,
      IReadOnlyDataLabelCollection<TData>
    where TData : IEquatable<TData>
{
    private Dictionary<ulong, int>? _dataIndexLookup;
    private Dictionary<string, int>? _labelIndexLookup;

    private readonly Collection<DataLabel<TData>> _dataLabels;

    public ReadOnlyDataLabelCollection()
    {
        _dataLabels = [];
    }

    public ReadOnlyDataLabelCollection(IEnumerable<DataLabel<TData>> dataLabels)
    {
        ArgumentNullException.ThrowIfNull(dataLabels);
        _dataLabels = [.. dataLabels];
    }

    private Dictionary<ulong, int> DataIndexLookup
    {
        get
        {
            if (_dataIndexLookup is not null)
            {
                return _dataIndexLookup;
            }

            _dataIndexLookup = new Dictionary<ulong, int>(_dataLabels.Count);

            for (var i = 0; i < _dataLabels.Count; i++)
            {
                _dataIndexLookup.Add(GetHash(_dataLabels[i].Data), i);
            }

            return _dataIndexLookup;
        }
    }

    private Dictionary<string, int> LabelIndexLookup
    {
        get
        {
            if (_labelIndexLookup is not null)
            {
                return _labelIndexLookup;
            }

            _labelIndexLookup = new Dictionary<string, int>(_dataLabels.Count);

            for (var i = 0; i < _dataLabels.Count; i++)
            {
                _labelIndexLookup.Add(_dataLabels[i].Label, i);
            }

            return _labelIndexLookup;
        }
    }

    public string this[TData data]
    {
        get
        {
            if (_dataLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return _dataLabels[DataIndexLookup[GetHash(data)]].Label;
        }
    }

    public int Count => _dataLabels.Count;

    public bool Contains(DataLabel<TData> item)
    {
        if (_dataLabels.Count == 0)
        {
            return false;
        }

        var key = GetHash(item.Data);

        if (!DataIndexLookup.TryGetValue(key, out var index))
        {
            return false;
        }

        return _dataLabels[index].Equals(item);
    }

    public bool ContainsLabel(string label)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_dataLabels.Count == 0)
        {
            return false;
        }

        return LabelIndexLookup.ContainsKey(label);
    }

    public bool ContainsDataValue(TData value)
    {
        if (_dataLabels.Count == 0)
        {
            return false;
        }

        var key = GetHash(value);

        return DataIndexLookup.ContainsKey(key);
    }

    public IEnumerator<DataLabel<TData>> GetEnumerator()
    {
        for (var i = 0; i < _dataLabels.Count; i++)
        {
            yield return _dataLabels[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool TryGetLabel(TData data, out string label)
    {
        if (_dataLabels.Count == 0)
        {
            label = string.Empty;
            return false;
        }

        var key = GetHash(data);

        if (DataIndexLookup.TryGetValue(key, out var index))
        {
            label = _dataLabels[index].Label;
            return true;
        }

        label = string.Empty;
        return false;
    }

    public bool TryGetDataValue(string label, out TData data)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_dataLabels.Count == 0)
        {
            data = default!;
            return false;
        }

        for (var i = 0; i < _dataLabels.Count; i++)
        {
            if (_dataLabels[i].Label == label)
            {
                data = _dataLabels[i].Data;
                return true;
            }
        }

        data = default!;
        return false;
    }
}
