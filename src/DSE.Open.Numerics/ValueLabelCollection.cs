// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Type-erased base for <see cref="ValueLabelCollection{T}"/>. Most callers
/// should work with the generic form; the non-generic base exists primarily
/// for serialization.
/// </summary>
[JsonConverter(typeof(ValueLabelCollectionJsonConverter))]
public abstract class ValueLabelCollection : IReadOnlyValueLabelCollection
{
    /// <summary>Initializes the base class.</summary>
    protected internal ValueLabelCollection()
    {
    }
}

/// <summary>
/// A mutable collection of human-readable labels associated with values of type
/// <typeparamref name="T"/>, used by a <see cref="Series{T}"/> to map data values to
/// display labels.
/// </summary>
/// <remarks>
/// A <see cref="ValueLabelCollection{T}"/> is attached to a <see cref="Series{T}"/>
/// by reference; later mutation of the collection is visible to every series it was
/// attached to. The series does not subscribe to changes, so removing labels,
/// clearing the collection, or replacing a label via remove-and-add does not
/// invalidate any series that still references the collection. (The collection
/// does not support in-place relabelling of an existing value; setting the
/// indexer for a value that already has a label throws.) Pass <c>copy: true</c>
/// to <see cref="Series{T}.Slice(int, int, bool)"/> to take an isolated copy.
/// </remarks>
public sealed class ValueLabelCollection<T> : ValueLabelCollection, IValueLabelCollection<T>
    where T : IEquatable<T>
{
    private Dictionary<T, int>? _valueIndexLookup;
    private Dictionary<string, int>? _labelIndexLookup;
    private readonly Collection<ValueLabel<T>> _valueLabels;

    /// <summary>Creates an empty collection.</summary>
    public ValueLabelCollection() : this([])
    {
    }

    /// <summary>Creates a collection seeded from <paramref name="dataLabels"/>.</summary>
    public ValueLabelCollection(IEnumerable<ValueLabel<T>> dataLabels) : this([.. dataLabels])
    {
    }

    internal ValueLabelCollection(Collection<ValueLabel<T>> valueLabels)
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        _valueLabels = valueLabels;
    }

    /// <summary>
    /// Gets or sets the label for <paramref name="data"/>. Get throws when no label
    /// is registered for the value; set adds a new label (cannot rename an existing
    /// label — use <see cref="Remove(ValueLabel{T})"/> first to replace).
    /// </summary>
    /// <exception cref="KeyNotFoundException">No label is registered for <paramref name="data"/> on get.</exception>
    /// <exception cref="ArgumentException">A label is already registered for the value on set.</exception>
    public string this[T data]
    {
        get
        {
            if (_valueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Data {data} not found.");
            }

            return _valueLabels[ValueIndexLookup[data]].Label;
        }
        set => Add(new ValueLabel<T>(data, value));
    }

    /// <summary>Gets the number of value-label pairs in the collection.</summary>
    public int Count => _valueLabels.Count;

    bool ICollection<ValueLabel<T>>.IsReadOnly => false;

    /// <summary>Adds <paramref name="label"/> to the collection.</summary>
    /// <exception cref="ArgumentException">A label is already registered for <c>label.Value</c>.</exception>
    public void Add(ValueLabel<T> label)
    {
        if (ValueIndexLookup.ContainsKey(label.Value))
        {
            throw new ArgumentException($"Label already included for label {label}", nameof(label));
        }

        ValueIndexLookup.Add(label.Value, _valueLabels.Count);
        LabelIndexLookup.Add(label.Label, _valueLabels.Count);

        _valueLabels.Add(label);
    }

    /// <summary>Adds <paramref name="data"/> with the given <paramref name="label"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="label"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">A label is already registered for <paramref name="data"/>.</exception>
    public void Add(T data, string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        Add((data, label));
    }

    /// <summary>Returns a read-only snapshot of this collection.</summary>
    public ReadOnlyValueLabelCollection<T> AsReadOnly()
    {
        return new ReadOnlyValueLabelCollection<T>(_valueLabels.AsReadOnly());
    }

    /// <summary>Removes all value-label pairs.</summary>
    public void Clear()
    {
        _valueLabels.Clear();
        _valueIndexLookup?.Clear();
        _labelIndexLookup?.Clear();
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="item"/> matches a registered value-label pair.</summary>
    public bool Contains(ValueLabel<T> item)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        if (!ValueIndexLookup.TryGetValue(item.Value, out var index))
        {
            return false;
        }

        return _valueLabels[index].Equals(item);
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="label"/> is registered for any value.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="label"/> is <see langword="null"/>.</exception>
    public bool ContainsLabel(string label)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_valueLabels.Count == 0)
        {
            return false;
        }

        return LabelIndexLookup.ContainsKey(label);
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> has any registered label.</summary>
    public bool ContainsValue(T value)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        return ValueIndexLookup.ContainsKey(value);
    }

    /// <summary>Returns an enumerator over the value-label pairs.</summary>
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

    /// <summary>Removes <paramref name="item"/>; returns <see langword="true"/> when it was found and removed.</summary>
    public bool Remove(ValueLabel<T> item)
    {
        if (_valueLabels.Count == 0)
        {
            return false;
        }

        if (!ValueIndexLookup.TryGetValue(item.Value, out var index))
        {
            return false;
        }

        if (!_valueLabels[index].Equals(item))
        {
            return false;
        }

        _ = ValueIndexLookup.Remove(item.Value);
        _ = LabelIndexLookup.Remove(_valueLabels[index].Label);

        _valueLabels.RemoveAt(index);

        for (var i = index; i < _valueLabels.Count; i++)
        {
            ValueIndexLookup[_valueLabels[i].Value] = i;
            LabelIndexLookup[_valueLabels[i].Label] = i;
        }

        return true;
    }

    /// <summary>
    /// Tries to find the label registered for <paramref name="value"/>. Returns
    /// <see langword="true"/> on hit (with <paramref name="label"/> set to the
    /// label) and <see langword="false"/> on miss (with <paramref name="label"/>
    /// set to <see cref="string.Empty"/>).
    /// </summary>
    public bool TryGetLabel(T value, out string label)
    {
        if (_valueLabels.Count == 0)
        {
            label = string.Empty;
            return false;
        }

        if (ValueIndexLookup.TryGetValue(value, out var index))
        {
            label = _valueLabels[index].Label;
            return true;
        }

        label = string.Empty;
        return false;
    }

    /// <summary>
    /// Tries to find the value registered with <paramref name="label"/>. Returns
    /// <see langword="true"/> on hit (with <paramref name="value"/> set to the
    /// value) and <see langword="false"/> on miss.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="label"/> is <see langword="null"/>.</exception>
    public bool TryGetValue(string label, out T value)
    {
        ArgumentNullException.ThrowIfNull(label);

        if (_valueLabels.Count == 0)
        {
            value = default!;
            return false;
        }

        if (LabelIndexLookup.TryGetValue(label, out var index))
        {
            value = _valueLabels[index].Value;
            return true;
        }

        value = default!;
        return false;
    }

    private Dictionary<T, int> ValueIndexLookup
    {
        get
        {
            if (_valueIndexLookup is not null)
            {
                return _valueIndexLookup;
            }

            _valueIndexLookup = new Dictionary<T, int>(_valueLabels.Count, EqualityComparer<T>.Default);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                _valueIndexLookup.Add(_valueLabels[i].Value, i);
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

#pragma warning disable IDE0028 // Simplify collection initialization
            _labelIndexLookup = new(_valueLabels.Count);
#pragma warning restore IDE0028 // Simplify collection initialization


            for (var i = 0; i < _valueLabels.Count; i++)
            {
                _labelIndexLookup.Add(_valueLabels[i].Label, i);
            }

            return _labelIndexLookup;
        }
    }
}
