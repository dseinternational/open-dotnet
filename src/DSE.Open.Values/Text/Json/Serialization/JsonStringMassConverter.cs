// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Values.Units;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonStringMassConverter : JsonConverter<Mass>
{
    public static readonly JsonStringMassConverter Default = new();

    public override Mass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (Mass.TryParse(value, out var length))
            {
                return length;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Mass value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
