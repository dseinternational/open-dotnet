// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON serialization helper for <see cref="IReadOnlyDataFrame"/>. Writes
/// <c>{ "name": ..., "columns": [ Series, Series, ... ] }</c>; each column is
/// emitted via <see cref="SeriesJsonWriter"/>.
/// </summary>
public static class DataFrameJsonWriter
{
    /// <summary>Writes <paramref name="value"/> to <paramref name="writer"/>.</summary>
    /// <exception cref="ArgumentNullException">Either <paramref name="writer"/> or <paramref name="value"/> is <see langword="null"/>.</exception>
    public static void Write(Utf8JsonWriter writer, IReadOnlyDataFrame value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        if (value.Name is not null)
        {
            writer.WriteString(NumericsPropertyNames.Name, value.Name);
        }

        writer.WritePropertyName(NumericsPropertyNames.Columns);

        writer.WriteStartArray();

        foreach (var column in value.GetReadOnlySeriesEnumerable())
        {
            SeriesJsonWriter.Write(writer, column, options);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
