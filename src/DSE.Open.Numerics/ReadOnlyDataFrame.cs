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
public sealed class ReadOnlyDataFrame : IReadOnlyList<ReadOnlySeries>, IReadOnlyDataFrame
{
    public static readonly ReadOnlyDataFrame Empty = new();

    private readonly ReadOnlyCollection<ReadOnlySeries> _columns;

    private ReadOnlyDataFrame() : this([], null)
    {
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlySeries> columns) : this(columns, null)
    {
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlySeries> columns, string? name)
    {
        ArgumentNullException.ThrowIfNull(columns);

        Name = name;

        _columns = columns;
    }

    public ReadOnlySeries this[int index] => _columns[index];

    public ReadOnlySeries? this[string name] => _columns.FirstOrDefault(s => s.Name == name);

    IReadOnlySeries? IReadOnlyDataFrame.this[string name] => this[name];

    IReadOnlySeries IReadOnlyDataFrame.this[int index] => this[index];

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    public int Count => _columns.Count;

    public ReadOnlyDataFrameRowCollection Rows => new(this);

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

    IEnumerable<IReadOnlySeries> IReadOnlyDataFrame.GetReadOnlySeriesEnumerable()
    {
        return this;
    }
}
