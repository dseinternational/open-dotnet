// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Series"/>).
/// </summary>
[JsonConverter(typeof(ReadOnlyDataFrameJsonConverter))]
[CollectionBuilder(typeof(ReadOnlyDataFrame), nameof(Create))]
public class ReadOnlyDataFrame : IReadOnlyDataFrame
{
    public static readonly ReadOnlyDataFrame Empty = new();

    private readonly ReadOnlyCollection<ReadOnlySeries> _columns;
    private readonly ReadOnlyCollection<string> _columnNames;

    private ReadOnlyDataFrame() : this([])
    {
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlySeries> columns, string? name = null, ReadOnlyCollection<string>? columnNames = null)
    {
        ArgumentNullException.ThrowIfNull(columns);

        if (columnNames is not null && columns.Count != columnNames.Count)
        {
            throw new ArgumentException("Columns and column names must have the same count.");
        }

        Name = name;
        _columns = columns;
        _columnNames = columnNames is not null ? columnNames : [];
    }

    public ReadOnlySeries this[int index] => _columns[index];

    public ReadOnlySeries? this[string name]
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
    }

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    public IReadOnlyList<string> ColumnNames => _columnNames;

    public int Count => _columns.Count;

    IReadOnlySeries IReadOnlyList<IReadOnlySeries>.this[int index] => throw new NotImplementedException();

    IReadOnlySeries? IReadOnlyDataFrame.this[string name] => throw new NotImplementedException();

    public IEnumerator<ReadOnlySeries> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    public static ReadOnlyDataFrame Create(ReadOnlySpan<ReadOnlySeries> columns)
    {
        return new ReadOnlyDataFrame([.. columns.ToArray()]);
    }

    IEnumerator<IReadOnlySeries> IEnumerable<IReadOnlySeries>.GetEnumerator()
    {
        return GetEnumerator();
    }
}
