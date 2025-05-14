// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Hashing;

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary values [ { "label": data } ]

public sealed class DataLabelCollection<TData> : IDataLabelCollection<TData>
    where TData : IEquatable<TData>
{
    private Dictionary<ulong, int>? _dataIndexLookup;
    private Dictionary<string, int>? _labelIndexLookup;

    private readonly Collection<DataLabel<TData>> _dataLabels;

    public DataLabelCollection()
    {
        _dataLabels = [];
    }

    public DataLabelCollection(IEnumerable<DataLabel<TData>> dataLabels)
    {
        ArgumentNullException.ThrowIfNull(dataLabels);
        _dataLabels = [.. dataLabels];
    }

    private static ulong GetHash(TData data)
    {
        if (data is IRepeatableHash64 hash64)
        {
            return hash64.GetRepeatableHashCode();
        }

        return HashU64_SplitMix(data.GetHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong HashU64_SplitMix(int value)
    {
        unchecked
        {
            var z = (uint)value + 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            z ^= z >> 31;
            return z;
        }
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
        set => Add(new DataLabel<TData>(data, value));
    }

    public int Count => _dataLabels.Count;

    bool ICollection<DataLabel<TData>>.IsReadOnly => false;

    public void Add(DataLabel<TData> label)
    {
        var key = GetHash(label.Data);

        if (DataIndexLookup.ContainsKey(key))
        {
            throw new ArgumentException($"Label already included for label {label}", nameof(label));
        }

        _dataLabels.Add(label);

        DataIndexLookup.Add(key, _dataLabels.Count - 1);
        LabelIndexLookup.Add(label.Label, _dataLabels.Count - 1);
    }

    public void Add(TData data, string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        Add((data, label));
    }

    public void Clear()
    {
        _dataLabels.Clear();
        _dataIndexLookup?.Clear();
        _labelIndexLookup?.Clear();
    }

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

    public bool ContainsDataValue(TData data)
    {
        if (_dataLabels.Count == 0)
        {
            return false;
        }

        var key = GetHash(data);

        return DataIndexLookup.ContainsKey(key);
    }

    void ICollection<DataLabel<TData>>.CopyTo(DataLabel<TData>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < _dataLabels.Count)
        {
            throw new ArgumentException(
                "The destination array is not long enough to copy all the items in the collection.");
        }

        for (var i = 0; i < _dataLabels.Count; i++)
        {
            array[arrayIndex + i] = _dataLabels[i];
        }
    }

    public IEnumerator<DataLabel<TData>> GetEnumerator()
    {
        for (var i = 0; i < _dataLabels.Count; i++)
        {
            yield return _dataLabels[i];
        }
    }

    public bool Remove(DataLabel<TData> item)
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

        if (!_dataLabels[index].Equals(item))
        {
            return false;
        }

        var label = _dataLabels[index].Label;

        _ = DataIndexLookup.Remove(key);
        _ = LabelIndexLookup.Remove(label);

        _dataLabels.RemoveAt(index);

        for (var i = index; i < _dataLabels.Count; i++)
        {
            DataIndexLookup[GetHash(_dataLabels[i].Data)] = i;
            LabelIndexLookup[label] = i;
        }

        return true;
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

    public bool TryGetData(string label, out TData data)
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
