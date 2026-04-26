// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Type-erased base for <see cref="ReadOnlyValueLabelCollection{T}"/>. Most callers
/// should work with the generic form; the non-generic base exists primarily for
/// serialization and runtime polymorphism.
/// </summary>
[JsonConverter(typeof(ReadOnlyValueLabelCollectionJsonConverter))]
public abstract class ReadOnlyValueLabelCollection : IReadOnlyValueLabelCollection
{
    /// <summary>Initializes the base class.</summary>
    protected internal ReadOnlyValueLabelCollection()
    {
    }

    /// <summary>Creates a read-only collection from <paramref name="valueLabels"/>.</summary>
    public static ReadOnlyValueLabelCollection<T> Create<T>(IEnumerable<ValueLabel<T>> valueLabels)
        where T : IEquatable<T>
    {
        if (valueLabels is ReadOnlyCollection<ValueLabel<T>> readOnlyValueLabels)
        {
            return new ReadOnlyValueLabelCollection<T>(readOnlyValueLabels);
        }

        return new ReadOnlyValueLabelCollection<T>(valueLabels);
    }

    /// <summary>Collection-initializer-friendly factory; copies <paramref name="span"/> into a fresh array.</summary>
    public static ReadOnlyValueLabelCollection<T> Create<T>(ReadOnlySpan<ValueLabel<T>> span)
        where T : IEquatable<T>
    {
        if (span.Length == 0)
        {
            return ReadOnlyValueLabelCollection<T>.Empty;
        }

        var labels = new ValueLabel<T>[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            labels[i] = span[i];
        }

        return new ReadOnlyValueLabelCollection<T>(labels);
    }
}

/// <summary>
/// A read-only collection of <see cref="ValueLabel{T}"/> pairs mapping values of
/// type <typeparamref name="T"/> to display labels. Used by <see cref="ReadOnlySeries{T}"/>
/// to expose a stable view of the labels attached at construction time.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
[CollectionBuilder(typeof(ReadOnlyValueLabelCollection), nameof(Create))]
public sealed class ReadOnlyValueLabelCollection<T>
    : ReadOnlyValueLabelCollection,
      IReadOnlyValueLabelCollection<T>
    where T : IEquatable<T>
{
    /// <summary>The shared empty collection.</summary>
    public static readonly ReadOnlyValueLabelCollection<T> Empty = new([]);

    private IReadOnlyDictionary<T, int>? _valueIndexLookup;
    private IReadOnlyDictionary<string, int>? _labelIndexLookup;
    private readonly ReadOnlyCollection<ValueLabel<T>> _valueLabels;

    /// <summary>Creates a read-only collection seeded from <paramref name="valueLabels"/>.</summary>
    public ReadOnlyValueLabelCollection(IEnumerable<ValueLabel<T>> valueLabels) : this([.. valueLabels])
    {
    }

    internal ReadOnlyValueLabelCollection(ReadOnlyCollection<ValueLabel<T>> valueLabels)
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        _valueLabels = valueLabels;
    }

    /// <summary>Gets the label registered for <paramref name="value"/>.</summary>
    /// <exception cref="KeyNotFoundException">No label is registered for <paramref name="value"/>.</exception>
    public string this[T value]
    {
        get
        {
            if (_valueLabels.Count == 0)
            {
                throw new KeyNotFoundException($"Value {value} not found.");
            }

            return _valueLabels[ValueIndexLookup[value]].Label;
        }
    }

    /// <summary>Gets the number of value-label pairs.</summary>
    public int Count => _valueLabels.Count;

    /// <summary>Returns <see langword="true"/> when <paramref name="item"/> matches a registered pair.</summary>
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

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> has a registered label.</summary>
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

    /// <summary>
    /// Tries to find the label registered for <paramref name="value"/>. Returns
    /// <see langword="true"/> on hit (with <paramref name="label"/> set) and
    /// <see langword="false"/> on miss (with <paramref name="label"/> set to
    /// <see cref="string.Empty"/>).
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
    /// <see langword="true"/> on hit (with <paramref name="value"/> set) and
    /// <see langword="false"/> on miss.
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

    private IReadOnlyDictionary<T, int> ValueIndexLookup
    {
        get
        {
            if (_valueIndexLookup is not null)
            {
                return _valueIndexLookup;
            }

            var valueIndexLookup = new Dictionary<T, int>(_valueLabels.Count, EqualityComparer<T>.Default);

            for (var i = 0; i < _valueLabels.Count; i++)
            {
                valueIndexLookup.Add(_valueLabels[i].Value, i);
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
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0306 // Simplify collection initialization
        return new(this);
#pragma warning restore IDE0306 // Simplify collection initialization
#pragma warning restore IDE0028 // Simplify collection initialization

    }
}
