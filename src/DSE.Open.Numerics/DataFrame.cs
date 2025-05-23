// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A set of related data, organised as a collection of columns (<see cref="Series"/>) of equal length,
/// optimised for serialization.
/// </summary>
[JsonConverter(typeof(DataFrameJsonConverter))]
[CollectionBuilder(typeof(DataFrame), nameof(Create))]
public sealed class DataFrame : IList<Series>, IReadOnlyDataFrame
{
    private readonly Collection<Series> _columns;

    public DataFrame() : this([], null)
    {
    }

    public DataFrame(string name) : this([], name)
    {
    }

    public DataFrame(Collection<Series> columns) : this(columns, null)
    {
    }

    public DataFrame(Collection<Series> columns, string? name)
    {
        ArgumentNullException.ThrowIfNull(columns);
        NumericsArgumentException.ThrowIfNotEqualLength(columns);

        Name = name;

        _columns = columns;

        for (var i = 0; i < _columns.Count; i++)
        {
            _columns[i].Name ??= i.ToStringInvariant();
        }
    }

    public Series? this[string name]
    {
        get => _columns.FirstOrDefault(s => s.Name == name);
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            var index = _columns.FindIndex(s => s.Name == name);

            if (index < 0)
            {
                _columns.Add(value);
            }
            else
            {
                _columns[index] = value;
                value.Name ??= index.ToStringInvariant();
            }
        }
    }

    public Series this[int index]
    {
        get => _columns[index];
        set => _columns[index] = value;
    }

    IReadOnlySeries? IReadOnlyDataFrame.this[string name] => this[name];

    IReadOnlySeries IReadOnlyDataFrame.this[int index] => this[index];

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the number of columns in the data frame.
    /// </summary>
    public int Count => _columns.Count;

    public DataFrameRowCollection Rows => new(this);

    bool ICollection<Series>.IsReadOnly => _columns.IsReadOnly;

    public ReadOnlyDataFrame AsReadOnly()
    {
        return new ReadOnlyDataFrame([.. _columns.Select(v => v.AsReadOnly())]);
    }

    public int IndexOf(Series item)
    {
        return _columns.IndexOf(item);
    }

    public void Insert(int index, Series item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _columns.Insert(index, item);
        item.Name ??= index.ToStringInvariant();
    }

    public void RemoveAt(int index)
    {
        _columns.RemoveAt(index);
    }

    public void Add(Series item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _columns.Add(item);
        item.Name ??= (_columns.Count - 1).ToStringInvariant();
    }

    public void Clear()
    {
        _columns.Clear();
    }

    public bool Contains(Series item)
    {
        return _columns.Contains(item);
    }

    public void CopyTo(Series[] array, int arrayIndex)
    {
        _columns.CopyTo(array, arrayIndex);
    }

    public bool Remove(Series item)
    {
        return _columns.Remove(item);
    }

    public IEnumerator<Series> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    IEnumerable<IReadOnlySeries> IReadOnlyDataFrame.GetReadOnlySeriesEnumerable()
    {
        return this;
    }

    [OverloadResolutionPriority(1)]
    public static DataFrame Create(Collection<Series> columns)
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new DataFrame(columns);
#pragma warning restore IDE0028 // Simplify collection initialization
    }

    public static DataFrame Create(Collection<Series> columns, string? name)
    {
        return new DataFrame(columns, name);
    }

    public static DataFrame Create(ReadOnlySpan<Series> columns)
    {
        return new DataFrame([.. columns.ToArray()]);
    }
}
