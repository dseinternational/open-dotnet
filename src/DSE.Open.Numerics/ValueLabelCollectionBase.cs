// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Hashing;

namespace DSE.Open.Numerics;

public abstract class ValueLabelCollectionBase<T> : IEnumerable<ValueLabel<T>>
    where T : IEquatable<T>
{
    private Dictionary<ulong, int>? _valueIndexLookup;
    private Dictionary<string, int>? _labelIndexLookup;
    private readonly Collection<ValueLabel<T>> _valueLabels;

    protected internal ValueLabelCollectionBase(Collection<ValueLabel<T>> valueLabels)
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        _valueLabels = valueLabels;
    }

    public int Count => _valueLabels.Count;

    protected Collection<ValueLabel<T>> ValueLabels => _valueLabels;

    protected void AddCore(ValueLabel<T> label)
    {
        var key = GetHash(label.Value);

        if (ValueIndexLookup.ContainsKey(key))
        {
            throw new ArgumentException($"Label already included for label {label}", nameof(label));
        }

        // before _valueLabels.Add
        ValueIndexLookup.Add(key, _valueLabels.Count);
        LabelIndexLookup.Add(label.Label, _valueLabels.Count);

        _valueLabels.Add(label);
    }

    protected void ClearCore()
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

        var key = GetHash(item.Value);

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

        var key = GetHash(value);

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

    protected bool RemoveCore(ValueLabel<T> item)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        var key = GetHash(item.Value);

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
            ValueIndexLookup[GetHash(_valueLabels[i].Value)] = i;
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

        var key = GetHash(value);

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

    protected Dictionary<ulong, int> ValueIndexLookup
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
                _valueIndexLookup.Add(GetHash(_valueLabels[i].Value), i);
            }

            return _valueIndexLookup;
        }
    }

    protected Dictionary<string, int> LabelIndexLookup
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

    protected static ulong GetHash(T value)
    {
        if (value is IRepeatableHash64 hash64)
        {
            return hash64.GetRepeatableHashCode();
        }

        return HashU64_SplitMix(value.GetHashCode());
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
}
