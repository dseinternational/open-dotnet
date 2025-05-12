// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Vector"/>).
/// </summary>
[JsonConverter(typeof(ReadOnlyDataFrameJsonConverter))]
public class ReadOnlyDataFrame : IReadOnlyList<Vector>
{
    private readonly Collection<Vector> _columnVectors;
    private readonly Collection<string> _columnNames;

    public ReadOnlyDataFrame() : this([])
    {
    }

    public ReadOnlyDataFrame(Collection<Vector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);

        _columnVectors = columns;
        _columnNames = new Collection<string>(columns.Count);

        for (var i = 0; i < columns.Count; i++)
        {
            _columnNames[i] = i.ToStringInvariant();
        }
    }

    public Vector this[int index] => _columnVectors[index];

    public Vector? this[string name]
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

    public IEnumerator<Vector> GetEnumerator()
    {
        return _columnVectors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columnVectors).GetEnumerator();
    }
}
