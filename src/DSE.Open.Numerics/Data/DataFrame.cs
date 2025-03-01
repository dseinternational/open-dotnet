// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Series"/>).
/// </summary>
public class DataFrame
{
    public DataFrame() : this([])
    {
    }

    [JsonConstructor]
    public DataFrame(Collection<Series> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        Columns = columns;
    }

    [JsonPropertyName("columns")]
    public Collection<Series> Columns { get; }

    public Series this[int index]
    {
        get => Columns[index];
        set => Columns[index] = value;
    }
}
