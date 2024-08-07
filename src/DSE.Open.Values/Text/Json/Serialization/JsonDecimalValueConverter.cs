// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonDecimalValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, decimal>
{
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TValue)reader.GetDecimal();
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((decimal)value);
    }
}
