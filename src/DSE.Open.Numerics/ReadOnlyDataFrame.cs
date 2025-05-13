// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Vector"/>).
/// </summary>
[JsonConverter(typeof(ReadOnlyDataFrameJsonConverter))]
[CollectionBuilder(typeof(ReadOnlyDataFrame), nameof(Create))]
public class ReadOnlyDataFrame : IReadOnlyList<ReadOnlyVector>
{
    public static readonly ReadOnlyDataFrame Empty = new();

    private readonly ReadOnlyCollection<ReadOnlyVector> _columns;
    private readonly ReadOnlyCollection<string> _columnNames;

    private ReadOnlyDataFrame() : this([])
    {
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlyVector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);

        _columns = columns;

        var names = new string[columns.Count];

        for (var i = 0; i < columns.Count; i++)
        {
            names[i] = i.ToStringInvariant();
        }

        _columnNames = [.. names];
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlyVector> columns, ReadOnlyCollection<string> columnNames)
    {
        ArgumentNullException.ThrowIfNull(columns);
        ArgumentNullException.ThrowIfNull(columnNames);

        if (columns.Count != columnNames.Count)
        {
            throw new ArgumentException("Vectors and column names must have the same count.");
        }

        _columns = columns;
        _columnNames = columnNames;
    }

    public ReadOnlyVector this[int index] => _columns[index];

    public ReadOnlyVector? this[string name]
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

    public IReadOnlyList<string> Columns => _columnNames;

    public int Count => _columns.Count;

    public IEnumerator<ReadOnlyVector> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    public static ReadOnlyDataFrame Create(ReadOnlySpan<ReadOnlyVector> columns)
    {
        return new ReadOnlyDataFrame([.. columns.ToArray()]);
    }
}
