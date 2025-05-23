// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ValueLabelCollectionJsonConverter))]
public abstract class ValueLabelCollection : IReadOnlyValueLabelCollection
{
    protected internal ValueLabelCollection()
    {
    }
}

public sealed class ValueLabelCollection<T> : ValueLabelCollection, IValueLabelCollection<T>
    where T : IEquatable<T>
{
    private Dictionary<ulong, int>? _valueIndexLookup;
    private Dictionary<string, int>? _labelIndexLookup;
    private readonly Collection<ValueLabel<T>> _valueLabels;

    public ValueLabelCollection() : this([])
    {
    }

    public ValueLabelCollection(IEnumerable<ValueLabel<T>> dataLabels) : this([.. dataLabels])
    {
    }

    internal ValueLabelCollection(Collection<ValueLabel<T>> valueLabels)
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        _valueLabels = valueLabels;
    }

    public string this[T data]
    {
        get
        {
            if (_valueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return _valueLabels[ValueIndexLookup[ValueLabelCollectionHelper.GetHash(data)]].Label;
        }
        set => Add(new ValueLabel<T>(data, value));
    }

    public int Count => _valueLabels.Count;

    bool ICollection<ValueLabel<T>>.IsReadOnly => false;

    public void Add(ValueLabel<T> label)
    {
        var key = ValueLabelCollectionHelper.GetHash(label.Value);

        if (ValueIndexLookup.ContainsKey(key))
        {
            throw new ArgumentException($"Label already included for label {label}", nameof(label));
        }

        // before _valueLabels.Add
        ValueIndexLookup.Add(key, _valueLabels.Count);
        LabelIndexLookup.Add(label.Label, _valueLabels.Count);

        _valueLabels.Add(label);
    }

    public void Add(T data, string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        Add((data, label));
    }

    public ReadOnlyValueLabelCollection<T> AsReadOnly()
    {
        return new ReadOnlyValueLabelCollection<T>(_valueLabels.AsReadOnly());
    }

    public void Clear()
    {
        _valueLabels.Clear();
        _valueIndexLookup?.Clear();
        _labelIndexLookup?.Clear();
    }

    public bool Contains(ValueLabel<T> item)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        var key = ValueLabelCollectionHelper.GetHash(item.Value);

        if (!ValueIndexLookup.TryGetValue(key, out var index))
        {
            return false;
        }

        return _valueLabels[index].Equals(item);
    }

    public bool ContainsLabel(string label)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_valueLabels.Count == 0)
        {
            return false;
        }

        return LabelIndexLookup.ContainsKey(label);
    }

    public bool ContainsValue(T value)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        var key = ValueLabelCollectionHelper.GetHash(value);

        return ValueIndexLookup.ContainsKey(key);
    }

    public IEnumerator<ValueLabel<T>> GetEnumerator()
    {
        for (var i = 0; i < _valueLabels.Count; i++)
        {
            yield return _valueLabels[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ICollection<ValueLabel<T>>.CopyTo(ValueLabel<T>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < _valueLabels.Count)
        {
            throw new ArgumentException(
                "The destination array is not long enough to copy all the items in the collection.");
        }

        for (var i = 0; i < _valueLabels.Count; i++)
        {
            array[arrayIndex + i] = _valueLabels[i];
        }
    }

    public bool Remove(ValueLabel<T> item)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        var key = ValueLabelCollectionHelper.GetHash(item.Value);

        if (!ValueIndexLookup.TryGetValue(key, out var index))
        {
            return false;
        }

        if (!_valueLabels[index].Equals(item))
        {
            return false;
        }

        var label = _valueLabels[index].Label;

        _ = ValueIndexLookup.Remove(key);
        _ = LabelIndexLookup.Remove(label);

        _valueLabels.RemoveAt(index);

        for (var i = index; i < _valueLabels.Count; i++)
        {
            ValueIndexLookup[ValueLabelCollectionHelper.GetHash(_valueLabels[i].Value)] = i;
            LabelIndexLookup[label] = i;
        }

        return true;
    }

    public bool TryGetLabel(T value, out string label)
    {
        if (_valueLabels.Count == 0)
        {
            label = string.Empty;
            return false;
        }

        var key = ValueLabelCollectionHelper.GetHash(value);

        if (ValueIndexLookup.TryGetValue(key, out var index))
        {
            label = _valueLabels[index].Label;
            return true;
        }

        label = string.Empty;
        return false;
    }

    public bool TryGetValue(string label, out T value)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_valueLabels.Count == 0)
        {
            value = default!;
            return false;
        }

        for (var i = 0; i < _valueLabels.Count; i++)
        {
            if (_valueLabels[i].Label == label)
            {
                value = _valueLabels[i].Value;
                return true;
            }
        }

        value = default!;
        return false;
    }

    private Dictionary<ulong, int> ValueIndexLookup
    {
        get
        {
            if (_valueIndexLookup is not null)
            {
                return _valueIndexLookup;
            }

            _valueIndexLookup = new Dictionary<ulong, int>(_valueLabels.Count);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                _valueIndexLookup.Add(ValueLabelCollectionHelper.GetHash(_valueLabels[i].Value), i);
            }

            return _valueIndexLookup;
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

            _labelIndexLookup = new Dictionary<string, int>(_valueLabels.Count);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                _labelIndexLookup.Add(_valueLabels[i].Label, i);
            }

            return _labelIndexLookup;
        }
    }
}
