// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class SeriesJsonWriter
{
    public static void Write(Utf8JsonWriter writer, IReadOnlySeries series, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(series);

        writer.WriteStartObject();

        if (series.Name is not null)
        {
            writer.WriteString(SeriesJsonPropertyNames.Name, series.Name);
        }

        // labels?

        writer.WritePropertyName(SeriesJsonPropertyNames.Data);

        VectorJsonWriter.WriteVector(writer, series.Vector, options);

        writer.WriteEndObject();
    }
}
