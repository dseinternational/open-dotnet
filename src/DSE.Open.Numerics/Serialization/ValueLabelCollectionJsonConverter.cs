// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON converter for the type-erased <see cref="ValueLabelCollection"/>.
/// Read is currently not supported (the typed collection's converter handles
/// deserialization); write delegates to <see cref="ValueLabelCollectionJsonWriter"/>.
/// </summary>
public class ValueLabelCollectionJsonConverter : JsonConverter<ValueLabelCollection>
{
    /// <inheritdoc />
    /// <exception cref="NotImplementedException">Always — read of the type-erased base is not supported.</exception>
    public override ValueLabelCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization not implemented.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ValueLabelCollection value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ValueLabelCollectionJsonWriter.WriteCollection(writer, value, options);
    }
}
