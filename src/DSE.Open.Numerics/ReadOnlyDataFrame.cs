// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Series"/>).
/// </summary>
[JsonConverter(typeof(ReadOnlyDataFrameJsonConverter))]
public class ReadOnlyDataFrame : IReadOnlyList<Series>
{
    public static readonly ReadOnlyDataFrame Empty = new();

    private readonly ReadOnlyCollection<Series> _columnVectors;
    private readonly ReadOnlyCollection<string> _columnNames;

    private ReadOnlyDataFrame() : this([])
    {
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<Series> vectors)
    {
        ArgumentNullException.ThrowIfNull(vectors);

        _columnVectors = vectors;

        var names = new string[vectors.Count];

        for (var i = 0; i < vectors.Count; i++)
        {
            names[i] = i.ToStringInvariant();
        }

        _columnNames = [.. names];
    }

    public ReadOnlyDataFrame(ReadOnlyCollection<Series> vectors, ReadOnlyCollection<string> columnNames)
    {
        ArgumentNullException.ThrowIfNull(vectors);
        ArgumentNullException.ThrowIfNull(columnNames);

        if (vectors.Count != columnNames.Count)
        {
            throw new ArgumentException("Vectors and column names must have the same count.");
        }

        _columnVectors = vectors;
        _columnNames = columnNames;
    }

    public Series this[int index] => _columnVectors[index];

    public Series? this[string name]
    {
        get
        {
            var index = _columnNames.IndexOf(name);

            if (index < 0)
            {
                return null;
            }

            return _columnVectors[index];
        }
    }

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    public IReadOnlyList<string> Columns => _columnNames;

    public int Count => _columnVectors.Count;

    public IEnumerator<Series> GetEnumerator()
    {
        return _columnVectors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columnVectors).GetEnumerator();
    }
}
