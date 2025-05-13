// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(DataFrameJsonConverter))]
[CollectionBuilder(typeof(DataFrame), nameof(Create))]
public class DataFrame : IDataFrame
{
    private readonly Collection<Vector> _columns;
    private readonly Collection<string> _columnNames;

    public DataFrame(string? name = null, Collection<string>? columnNames = null) : this([], name, columnNames)
    {
    }

    public DataFrame(Collection<Vector> columns, string? name = null, Collection<string>? columnNames = null)
    {
        ArgumentNullException.ThrowIfNull(columns);

        if (columnNames is not null && columns.Count != columnNames.Count)
        {
            throw new ArgumentException("Columns and column names must have the same count.");
        }

        Name = name;
        _columns = columns;
        _columnNames = columnNames is not null ? columnNames : new(columns.Count);
    }

    public Vector? this[string name]
    {
        get
        {
            var index = _columnNames.IndexOf(name);

            if (index < 0)
            {
                return null;
            }

            return _columns[index];
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            var index = _columnNames.IndexOf(name);

            if (index < 0)
            {
                _columnNames.Add(name);
                _columns.Add(value);
            }
            else
            {
                _columns[index] = value;
            }
        }
    }

    public Vector this[int index]
    {
        get => _columns[index];
        set => _columns[index] = value;
    }

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; set; }

    public IReadOnlyList<string> ColumnNames => _columnNames;

    /// <summary>
    /// Gets the number of columns in the data frame.
    /// </summary>
    public int Count => _columns.Count;

    public bool IsReadOnly => false;

    public ReadOnlyDataFrame AsReadOnly()
    {
        return new ReadOnlyDataFrame([.. _columns.Select(v => v.AsReadOnly())]);
    }

    public int IndexOf(Vector item)
    {
        return _columns.IndexOf(item);
    }

    public void Insert(int index, Vector item)
    {
        _columns.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _columns.RemoveAt(index);
    }

    public void Add(Vector item)
    {
        _columns.Add(item);
    }

    public void Clear()
    {
        _columns.Clear();
    }

    public bool Contains(Vector item)
    {
        return _columns.Contains(item);
    }

    public void CopyTo(Vector[] array, int arrayIndex)
    {
        _columns.CopyTo(array, arrayIndex);
    }

    public bool Remove(Vector item)
    {
        return _columns.Remove(item);
    }

    public IEnumerator<Vector> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    public static DataFrame Create(ReadOnlySpan<Vector> columns)
    {
        return new DataFrame([.. columns.ToArray()]);
    }
}
