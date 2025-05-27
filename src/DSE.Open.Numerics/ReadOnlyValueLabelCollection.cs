// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ReadOnlyValueLabelCollectionJsonConverter))]
public abstract class ReadOnlyValueLabelCollection : IReadOnlyValueLabelCollection
{
    protected internal ReadOnlyValueLabelCollection()
    {
    }

    public static ReadOnlyValueLabelCollection<T> Create<T>(IEnumerable<ValueLabel<T>> valueLabels)
        where T : IEquatable<T>
    {
        if (valueLabels is ReadOnlyCollection<ValueLabel<T>> readOnlyValueLabels)
        {
            return new ReadOnlyValueLabelCollection<T>(readOnlyValueLabels);
        }

        return new ReadOnlyValueLabelCollection<T>(valueLabels);
    }

    public static ReadOnlyValueLabelCollection<T> Create<T>(ReadOnlySpan<ValueLabel<T>> span)
        where T : IEquatable<T>
    {
        if (span.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyValueLabelCollection<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        var labels = new ValueLabel<T>[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            labels[i] = span[i];
        }

        return new ReadOnlyValueLabelCollection<T>(labels);
    }
}

[CollectionBuilder(typeof(ReadOnlyValueLabelCollection), nameof(Create))]
public sealed class ReadOnlyValueLabelCollection<T>
    : ReadOnlyValueLabelCollection,
      IReadOnlyValueLabelCollection<T>
    where T : IEquatable<T>
{
    public static readonly ReadOnlyValueLabelCollection<T> Empty = new([]);

    private IReadOnlyDictionary<ulong, int>? _valueIndexLookup;
    private IReadOnlyDictionary<string, int>? _labelIndexLookup;
    private readonly ReadOnlyCollection<ValueLabel<T>> _valueLabels;

    public ReadOnlyValueLabelCollection(IEnumerable<ValueLabel<T>> valueLabels) : this([.. valueLabels])
    {
    }

    internal ReadOnlyValueLabelCollection(ReadOnlyCollection<ValueLabel<T>> valueLabels)
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        _valueLabels = valueLabels;
    }

    public string this[T value]
    {
        get
        {
            if (_valueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Value {value} not found.");
            }

            return _valueLabels[ValueIndexLookup[ValueLabelCollectionHelper.GetHash(value)]].Label;
        }
    }

    public int Count => _valueLabels.Count;

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

    private IReadOnlyDictionary<ulong, int> ValueIndexLookup
    {
        get
        {
            if (_valueIndexLookup is not null)
            {
                return _valueIndexLookup;
            }

            var valueIndexLookup = new Dictionary<ulong, int>(_valueLabels.Count);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                valueIndexLookup.Add(ValueLabelCollectionHelper.GetHash(_valueLabels[i].Value), i);
            }

            _valueIndexLookup = valueIndexLookup;

            return _valueIndexLookup;
        }
    }

    private IReadOnlyDictionary<string, int> LabelIndexLookup
    {
        get
        {
            if (_labelIndexLookup is not null)
            {
                return _labelIndexLookup;
            }

            var labelIndexLookup = new Dictionary<string, int>(_valueLabels.Count);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                labelIndexLookup.Add(_valueLabels[i].Label, i);
            }

            _labelIndexLookup = labelIndexLookup;

            return _labelIndexLookup;
        }
    }

    /// <summary>
    /// Creates a mutable <see cref="ValueLabelCollection{T}"/> from this read-only collection.
    /// </summary>
    public ValueLabelCollection<T> ToValueLabelCollection()
    {
        return new ValueLabelCollection<T>(this);
    }
}
