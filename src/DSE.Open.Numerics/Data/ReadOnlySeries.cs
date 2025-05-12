// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// A serializable sequence of data with a label.
/// </summary>
[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class ReadOnlySeries : IReadOnlySeries
{
    protected ReadOnlySeries(string? name, IReadOnlyVector data, IReadOnlyDictionary<string, Variant>? annotations)
    {
        ArgumentNullException.ThrowIfNull(data);

        Name = name;
        Data = data;
        Annotations = annotations;
    }

    public string? Name { get; }

    public IReadOnlyVector Data { get; }

    public IReadOnlyDictionary<string, Variant>? Annotations { get; }

    IReadOnlyVector IReadOnlySeries.Data => Data;
}
