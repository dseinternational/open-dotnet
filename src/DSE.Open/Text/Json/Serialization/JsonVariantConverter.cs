// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public class JsonVariantConverter : JsonConverter<Variant>
{
    public static readonly JsonVariantConverter Default = new();

    public override Variant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return Variant.Null;
        }

        if (reader.TokenType == JsonTokenType.True)
        {
            return true;
        }

        if (reader.TokenType == JsonTokenType.False)
        {
            return false;
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out var integer))
            {
                return integer;
            }

            return reader.GetDouble();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString() ?? string.Empty;
        }

        throw new JsonException("Unable to read Variant value.");
    }

    public override void Write(Utf8JsonWriter writer, Variant value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.IsNull)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.IsInteger)
        {
            writer.WriteNumberValue(value.Integer.Value);
            return;
        }

        if (value.IsFloatingPoint)
        {
            writer.WriteNumberValue(value.FloatingPoint.Value);
            return;
        }

        if (value.IsText)
        {
            writer.WriteStringValue(value.Text);
            return;
        }

        if (value.IsBoolean)
        {
            writer.WriteBooleanValue(value.Boolean.Value);
            return;
        }

        throw new JsonException("Unable to write Variant value.");
    }
}
