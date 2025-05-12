// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Vector"/>).
/// </summary>
public class DataFrame
{
    public DataFrame() : this([])
    {
    }

    [JsonConstructor]
    public DataFrame(Collection<Vector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        Columns = columns;
    }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; }

    [JsonPropertyName("columns")]
    public Collection<Vector> Columns { get; }

    public Vector this[int index]
    {
        get => Columns[index];
        set => Columns[index] = value;
    }

    public Vector? this[string name]
    {
        get => Columns.FirstOrDefault(s => s.Name == name);
        set
        {
            var index = Columns.FindIndex(s => s.Name == name);

            if (index >= 0)
            {
                if (value is not null)
                {
                    Columns[index] = value;
                }
                else
                {
                    Columns.RemoveAt(index);
                }
            }
            else
            {
                if (value is not null)
                {
                    Columns.Add(value);
                }
            }
        }
    }

    public bool TryGetColumn(string name, out Vector? column)
    {
        column = Columns.FirstOrDefault(s => s.Name == name);
        return column != null;
    }

    public bool TryGetColumn<T>(string name, out Vector<T>? column)
    {
        column = Columns.OfType<Vector<T>>().FirstOrDefault(s => s.Name == name);
        return column != null;
    }
}
