// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Vector"/>).
/// </summary>
public class ReadOnlyDataFrame : IReadOnlyList<Vector>
{
    private readonly Collection<Vector> _columns;

    public ReadOnlyDataFrame() : this([])
    {
    }

    public ReadOnlyDataFrame(Collection<Vector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        _columns = columns;
    }

    public Vector this[int index] => _columns[index];

    public Vector? this[string name] => null; // TODO

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    public int Count => _columns.Count;

    public IEnumerator<Vector> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }
}
