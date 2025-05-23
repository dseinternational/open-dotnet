// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class ReadOnlyValueLabelCollectionJsonConverter : JsonConverter<ReadOnlyValueLabelCollection>
{
    public override ReadOnlyValueLabelCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization not implemented.");
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyValueLabelCollection value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ValueLabelCollectionJsonWriter.WriteCollection(writer, value, options);
    }
}
