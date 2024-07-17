// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Values.Units;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonStringLengthConverter : JsonConverter<Length>
{
    public static readonly JsonStringLengthConverter Default = new();

    public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (Length.TryParse(value, out var result))
            {
                return result;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Length value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        Guard.IsNotDefault(value);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
