// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

// as per system json serializer, serialize like dictionary with label as property...

internal static class DataLabelCollectionJsonWriter
{
    public static void Write<T>(
        Utf8JsonWriter writer,
        IReadOnlyValueLabelCollection<T> labels,
        JsonSerializerOptions options)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(labels);

        writer.WriteStartObject();

        foreach (var dataLabel in labels)
        {
            writer.WritePropertyName(dataLabel.Label);
            // todo: write value
        }

        writer.WriteEndObject();
    }
}
